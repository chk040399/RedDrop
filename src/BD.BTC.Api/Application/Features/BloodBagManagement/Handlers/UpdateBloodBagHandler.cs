using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Application.DTOs;
using Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Application.Features.BloodBagManagement.Commands;
using Domain.ValueObjects;
using Application.Interfaces;
using Infrastructure.ExternalServices.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Domain.Events;

namespace Application.Features.BloodBagManagement.Handlers
{
    public class UpdateBloodBagHandler : IRequestHandler<UpdateBloodBagCommand, (BloodBagDTO? bloodBag, BaseException? err)>
    {
        private readonly IBloodBagRepository _bloodBagRepository;
        private readonly ILogger<UpdateBloodBagHandler> _logger;
        private readonly IGlobalStockRepository _globalStockRepository;
        private readonly IEventProducer _eventProducer;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly IBloodTransferCenterRepository _centerRepository;

        public UpdateBloodBagHandler(
            IBloodBagRepository bloodBagRepository, 
            ILogger<UpdateBloodBagHandler> logger,
            IGlobalStockRepository globalStockRepository,
            IEventProducer eventProducer,
            IOptions<KafkaSettings> kafkaSettings,
            IBloodTransferCenterRepository centerRepository)
        {
            _bloodBagRepository = bloodBagRepository;
            _logger = logger;
            _globalStockRepository = globalStockRepository;
            _eventProducer = eventProducer;
            _kafkaSettings = kafkaSettings;
            _centerRepository = centerRepository;
        }

        public async Task<(BloodBagDTO? bloodBag, BaseException? err)> Handle(UpdateBloodBagCommand command, CancellationToken ct)
        {
            try
            {
                // Get the original blood bag
                var bloodBag = await _bloodBagRepository.GetByIdAsync(command.Id);
                if (bloodBag == null)
                {
                    _logger.LogError("Blood bag not found");
                    throw new NotFoundException("No blood bag found with the provided ID", "Updating blood bag");
                }

                // Save original status for comparison
                var originalStatus = bloodBag.Status;
                var originalBloodType = bloodBag.BloodType;
                var originalBagType = bloodBag.BloodBagType;

                // Update the blood bag - Separate status update from other properties
                bloodBag.UpdateDetails(
                    command.BloodBagType,
                    command.BloodType,
                    command.ExpirationDate,
                    command.AcquiredDate,
                    null, // No DonorId update
                    command.RequestId);
                
                // Update status separately if provided
                if (command.Status != null)
                {
                    bloodBag.UpdateStatus(command.Status);
                }

                await _bloodBagRepository.UpdateAsync(bloodBag);
                _logger.LogInformation("Blood bag updated successfully");

                // Check if status has changed
                if (command.Status != null && !originalStatus.Equals(command.Status))
                {
                    _logger.LogInformation("Blood bag status changed from {OldStatus} to {NewStatus}", 
                        originalStatus.Value, command.Status.Value);
                    
                    // Handle global stock updates based on status change
                    await UpdateGlobalStockForStatusChange(
                        originalStatus, command.Status, 
                        originalBloodType, originalBagType,
                        bloodBag.BloodType, bloodBag.BloodBagType);
                }

                // Return the DTO
                var bloodBagDto = new BloodBagDTO
                {
                    Id = bloodBag.Id,
                    BloodType = bloodBag.BloodType,
                    BloodBagType = bloodBag.BloodBagType,
                    ExpirationDate = bloodBag.ExpirationDate,
                    AcquiredDate = bloodBag.AcquiredDate,
                    Status = bloodBag.Status,
                    DonorId = bloodBag.DonorId ?? Guid.Empty,
                    RequestId = bloodBag.RequestId
                };

                return (bloodBagDto, null);
            }
            catch (BaseException ex)
            {
                _logger.LogError("Error while updating blood bag: {Message}", ex.Message);
                return (null, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating blood bag");
                return (null, new InternalServerException(ex.Message, "updating blood bag"));
            }
        }

        private async Task UpdateGlobalStockForStatusChange(
            BloodBagStatus oldStatus, BloodBagStatus newStatus,
            BloodType oldBloodType, BloodBagType oldBagType,
            BloodType newBloodType, BloodBagType newBagType)
        {
            try
            {
                // If blood type or bag type changed, we need to update both old and new stocks
                bool typeChanged = !oldBloodType.Equals(newBloodType) || !oldBagType.Equals(newBagType);
                
                // Get the old stock
                var oldStock = await _globalStockRepository.GetByKeyAsync(oldBloodType, oldBagType);
                if (oldStock == null)
                {
                    _logger.LogWarning("Old global stock not found for {BloodType} {BloodBagType}", 
                        oldBloodType.Value, oldBagType.Value);
                    return;
                }

                // Handle old stock updates
                if (oldStatus.Value == BloodBagStatus.Ready().Value || 
                    oldStatus.Value == BloodBagStatus.Aquired().Value)
                {
                    // Decrement available count since it's no longer in this status
                    oldStock.DecrementAvailableCount(1);
                    _logger.LogInformation("Decremented available count for {BloodType} {BloodBagType}",
                        oldBloodType.Value, oldBagType.Value);
                }
                else if (oldStatus.Value == BloodBagStatus.Expired().Value)
                {
                    // Decrement expired count since it's no longer expired
                    oldStock.UpdateCounts(
                        oldStock.CountExpired - 1,
                        oldStock.CountExpiring,
                        oldStock.ReadyCount
                    );
                    _logger.LogInformation("Decremented expired count for {BloodType} {BloodBagType}",
                        oldBloodType.Value, oldBagType.Value);
                }

                await _globalStockRepository.UpdateAsync(oldStock);

                // If blood type or bag type changed, update the new stock too
                GlobalStock newStock = oldStock;
                if (typeChanged)
                {
                    newStock = await _globalStockRepository.GetByKeyAsync(newBloodType, newBagType);
                    if (newStock == null)
                    {
                        _logger.LogWarning("New global stock not found for {BloodType} {BloodBagType}", 
                            newBloodType.Value, newBagType.Value);
                        return;
                    }
                }

                // Handle new stock updates
                if (newStatus.Value == BloodBagStatus.Ready().Value || 
                    newStatus.Value == BloodBagStatus.Aquired().Value)
                {
                    // Increment available count for the new status
                    newStock.IncrementAvailableCount(1);
                    _logger.LogInformation("Incremented available count for {BloodType} {BloodBagType}",
                        newBloodType.Value, newBagType.Value);
                }
                else if (newStatus.Value == BloodBagStatus.Expired().Value)
                {
                    // Increment expired count for the new status
                    newStock.IncrementExpiredCount(1);
                    _logger.LogInformation("Incremented expired count for {BloodType} {BloodBagType}",
                        newBloodType.Value, newBagType.Value);
                }

                if (typeChanged)
                {
                    await _globalStockRepository.UpdateAsync(newStock);
                }

                // Publish Kafka message for both stocks
                await PublishGlobalStockEvent(oldBloodType, oldBagType, oldStock);
                if (typeChanged)
                {
                    await PublishGlobalStockEvent(newBloodType, newBagType, newStock);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating global stock for status change");
                // Don't rethrow - we don't want to fail the blood bag update if stock update fails
            }
        }

        private async Task PublishGlobalStockEvent(BloodType bloodType, BloodBagType bloodBagType, GlobalStock stock)
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
                
                await _eventProducer.ProduceAsync(topic,"global-stock");
                _logger.LogInformation("Published global stock update for {BloodType} {BloodBagType}", 
                    bloodType.Value, bloodBagType.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish global stock update event");
            }
        }
    }
}