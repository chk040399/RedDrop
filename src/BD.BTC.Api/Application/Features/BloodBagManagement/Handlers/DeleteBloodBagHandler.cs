using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.BloodBagManagement.Commands;
using Application.Interfaces;
using Domain.Events;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.ExternalServices.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Exceptions;

namespace Application.Features.BloodBagManagement.Handlers
{
    public class DeleteBloodBagHandler : IRequestHandler<DeleteBloodBagCommand, (BloodBagDTO? BloodBag, BaseException? err)>
    {
        private readonly IBloodBagRepository _bloodBagRepository;
        private readonly ILogger<DeleteBloodBagHandler> _logger;
        private readonly IGlobalStockRepository _globalStockRepository;
        private readonly IEventProducer _eventProducer;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly IBloodTransferCenterRepository _centerRepository;
        private readonly IRequestRepository _requestRepository;

        public DeleteBloodBagHandler(
            IBloodBagRepository bloodBagRepository, 
            ILogger<DeleteBloodBagHandler> logger,
            IGlobalStockRepository globalStockRepository,
            IEventProducer eventProducer,
            IOptions<KafkaSettings> kafkaSettings,
            IBloodTransferCenterRepository centerRepository,
            IRequestRepository requestRepository)
        {
            _bloodBagRepository = bloodBagRepository;
            _logger = logger;
            _globalStockRepository = globalStockRepository;
            _eventProducer = eventProducer;
            _kafkaSettings = kafkaSettings;
            _centerRepository = centerRepository;
            _requestRepository = requestRepository;
        }

        public async Task<(BloodBagDTO? BloodBag, BaseException? err)> Handle(DeleteBloodBagCommand BloodBag, CancellationToken cancellationToken)
        {
            try 
            {
                var bloodBag = await _bloodBagRepository.GetByIdAsync(BloodBag.Id);
                if (bloodBag == null)
                {
                    _logger.LogError("Blood bag with ID {BloodBagId} not found", BloodBag.Id);
                    return (null, new NotFoundException($"Blood bag {BloodBag.Id} not found", "delete blood bag"));
                }
                
                // Save blood bag data before soft deletion for updating stock and returning DTO
                var bloodBagType = bloodBag.BloodBagType;
                var bloodType = bloodBag.BloodType;
                var status = bloodBag.Status;
                var requestId = bloodBag.RequestId;
                
                // Create DTO before soft-deleting the blood bag
                var bloodBagDto = new BloodBagDTO
                {
                    Id = bloodBag.Id,
                    BloodBagType = bloodBag.BloodBagType,
                    BloodType = bloodBag.BloodType,
                    Status = bloodBag.Status,
                    ExpirationDate = bloodBag.ExpirationDate,
                    AcquiredDate = bloodBag.AcquiredDate,
                    DonorId = bloodBag.DonorId ?? Guid.Empty,
                    RequestId = bloodBag.RequestId
                };

                // If blood bag is associated with a request, update request's acquired quantity
                if (requestId.HasValue)
                {
                    var request = await _requestRepository.GetByIdAsync(requestId.Value);
                    if (request != null)
                    {
                        // Decrement acquired quantity and increment required quantity
                        request.DecrementAcquiredQty();
                        await _requestRepository.UpdateAsync(request);
                        
                        // Send Kafka message to update request
                        var updateRequestTopic = _kafkaSettings.Value.Topics["UpdateRequest"];
                        var updateRequestEvent = new UpdateRequestEvent(
                            request.Id,
                            null,
                            request.Status.Value,
                            request.AquiredQty,
                            request.RequiredQty,
                            null
                        );
                        await _eventProducer.ProduceAsync(updateRequestTopic, updateRequestEvent);
                        _logger.LogInformation("Sent request update event for request {RequestId}", request.Id);
                    }
                }

                // Soft delete the blood bag instead of physically removing it
                await _bloodBagRepository.DeleteAsync(bloodBag.Id);
                _logger.LogInformation("Blood bag with ID {BloodBagId} marked as deleted", BloodBag.Id);
                
                // Update global stock based on deleted blood bag status
                await UpdateGlobalStockForDeletedBag(bloodType, bloodBagType, status);

                return (bloodBagDto, null);
            }
            catch (BaseException ex)
            {
                _logger.LogError(ex, "Failed to delete blood bag {BloodBagId}", BloodBag.Id);
                return (null, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting blood bag {BloodBagId}", BloodBag.Id);
                return (null, new InternalServerException(ex.Message, "deleting blood bag"));
            }
        }
        
        private async Task UpdateGlobalStockForDeletedBag(
            BloodType bloodType, 
            BloodBagType bloodBagType, 
            BloodBagStatus status)
        {
            try
            {
                // Get the global stock for this blood type and bag type
                var stock = await _globalStockRepository.GetByKeyAsync(bloodType, bloodBagType);
                if (stock == null)
                {
                    _logger.LogWarning("Global stock not found for {BloodType} {BloodBagType} when deleting blood bag", 
                        bloodType.Value, bloodBagType.Value);
                    return;
                }

                // Update stock based on the status of the deleted blood bag
                if (status.Value == BloodBagStatus.Ready().Value || 
                    status.Value == BloodBagStatus.Aquired().Value)
                {
                    // Decrement available count since it's no longer available
                    stock.DecrementAvailableCount(1);
                    _logger.LogInformation("Decremented available count for {BloodType} {BloodBagType} after blood bag deletion",
                        bloodType.Value, bloodBagType.Value);
                }
                else if (status.Value == BloodBagStatus.Expired().Value)
                {
                    // Decrement expired count
                    stock.UpdateCounts(
                        stock.CountExpired - 1,
                        stock.CountExpiring,
                        stock.ReadyCount
                    );
                    _logger.LogInformation("Decremented expired count for {BloodType} {BloodBagType} after blood bag deletion",
                        bloodType.Value, bloodBagType.Value);
                }
                // We don't need to update for Used status as they're not counted in stock

                await _globalStockRepository.UpdateAsync(stock);
                
                // Publish Kafka message with updated stock information
                await PublishGlobalStockEvent(bloodType, bloodBagType, stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating global stock after blood bag deletion");
                // Don't rethrow - we don't want to fail the blood bag deletion if stock update fails
            }
        }
        
        private async Task PublishGlobalStockEvent(BloodType bloodType, BloodBagType bloodBagType, Domain.Entities.GlobalStock stock)
        {
            try
            {
                var center = await _centerRepository.GetPrimaryAsync();
                if (center == null)
                {
                    _logger.LogWarning("Blood transfer center not found when publishing stock update");
                    return;
                }
                
                var topic = _kafkaSettings.Value.Topics["GlobalStock"];
                var stockData = new GlobalStockData(
                    bloodType,
                    bloodBagType,
                    stock.ReadyCount + stock.CountExpiring + stock.CountExpired,
                    stock.ReadyCount,
                    stock.MinStock,
                    stock.CountExpired
                );
                
                var globalStockEvent = new GlobalStockEvent(
                    center.Id,
                    stockData
                );
                
                await _eventProducer.ProduceAsync(topic, globalStockEvent);
                _logger.LogInformation("Published global stock update for {BloodType} {BloodBagType} after blood bag deletion", 
                    bloodType.Value, bloodBagType.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish global stock update event");
            }
        }
    }
}