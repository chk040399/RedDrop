using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Application.DTOs;
using Application.Features.BloodBagManagement.Commands;
using Shared.Exceptions;
using Domain.ValueObjects;
using Domain.Events;
using Application.Interfaces;
using Infrastructure.ExternalServices.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using BD.BTC.Api.Converters;



namespace Application.Features.BloodBagManagement.Handlers
{
    public class CreateBloodBagHandler : IRequestHandler<CreateBloodBagCommand, (BloodBagDTO? bloodBag, BaseException? err)>
    {
        private readonly IBloodBagRepository _bloodBagRepository;
        private readonly ILogger<CreateBloodBagHandler> _logger;
        private readonly IEventProducer _eventProducer;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly IDonorRepository _donorRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IPledgeRepository _pledgeRepository;
        private readonly IGlobalStockRepository _globalStockRepository;
        private readonly IBloodTransferCenterRepository _centerRepository;

        public CreateBloodBagHandler(IBloodBagRepository bloodBagRepository, ILogger<CreateBloodBagHandler> logger, IEventProducer eventProducer,
            IRequestRepository requestRepository, IDonorRepository donorRepository, IPledgeRepository pledgeRepository, IOptions<KafkaSettings> kafkaSettings, IGlobalStockRepository globalStockRepository, IBloodTransferCenterRepository centerRepository)
        {
            _eventProducer = eventProducer;
            _requestRepository = requestRepository;
            _pledgeRepository = pledgeRepository;
            _bloodBagRepository = bloodBagRepository;
            _centerRepository = centerRepository;
            _donorRepository = donorRepository;
            _kafkaSettings = kafkaSettings;
            _logger = logger;
            _globalStockRepository = globalStockRepository;
        }

        public async Task<(BloodBagDTO? bloodBag, BaseException? err)> Handle(CreateBloodBagCommand bloodBag, CancellationToken cancellationToken)
        {
            try
            {
                var newBloodBag = new BloodBag(
                    bloodBag.BloodBagType,
                    bloodBag.BloodType,
                    bloodBag.Status,
                    bloodBag.ExpirationDate,
                    bloodBag.AcquiredDate,
                    bloodBag.DonorId,
                    bloodBag.RequestId);

                await _bloodBagRepository.AddAsync(newBloodBag);

                // Handle request and pledge updates (existing code)
                if (newBloodBag.RequestId != null)
                {
                    var request = await _requestRepository.GetByIdAsync(newBloodBag.RequestId.Value);
                    if (request == null)
                    {
                        throw new NotFoundException("Request not found", "fetching request updating pledge status");
                    }
                    var pledge = await _pledgeRepository.GetByDonorAndRequestIdAsync(newBloodBag.DonorId!.Value, newBloodBag.RequestId.Value);
                    if (pledge != null)
                    {
                        pledge.UpdateStatus(PledgeStatus.Fulfilled);
                        await _pledgeRepository.UpdateAsync(pledge);
                        request.UpdateAquiredQty();
                        await _requestRepository.UpdateAsync(request);
                        var updateRequestTopic = _kafkaSettings.Value.Topics["UpdateRequest"];
                        if (request.RequiredQty == 0)
                        {
                            var center = await _centerRepository.GetPrimaryAsync();
                            if (center == null)
                            {
                                throw new NotFoundException("Primary blood transfer center not found", "creating blood bag");
                            }
                            var requestResolvedEvent = new UpdateRequestEvent(
                                center.Id,
                                request.Id,
                                request.RequiredQty,
                                RequestStatusConverter.ToEnum(RequestStatus.Resolved()),
                                request.AquiredQty,null,
                                null);
                            await _eventProducer.ProduceAsync("update-request", requestResolvedEvent);
                        }
                        else
                        {
                             var center = await _centerRepository.GetPrimaryAsync();
                            var updateRequestEvent = new UpdateRequestEvent(
                                center.Id,
                                request.Id,
                                request.RequiredQty,
                                null,
                                request.AquiredQty,
                                null
                              , null);
                            await _eventProducer.ProduceAsync("update-request",updateRequestEvent);

                        }
                    }
                }

                // Set expiration date based on blood bag type if not provided
                if (newBloodBag.ExpirationDate == null)
                {
                    // Use the value comparison instead of direct reference comparison
                    if (newBloodBag.BloodBagType.Value == BloodBagType.Blood().Value)
                    {
                        newBloodBag.UpdateExpirationDate(DateTime.Now.AddDays(30)); // Blood expires after 30 days
                    }
                    else if (newBloodBag.BloodBagType.Value == BloodBagType.Plaquette().Value)
                    {
                        newBloodBag.UpdateExpirationDate(DateTime.Now.AddDays(5)); // Plaquette expires after 5 days
                    }
                    else if (newBloodBag.BloodBagType.Value == BloodBagType.Plasma().Value)
                    {
                        newBloodBag.UpdateExpirationDate(DateTime.Now.AddDays(5)); // Plasma expires after 5 days
                    }
                    else
                    {
                        newBloodBag.UpdateExpirationDate(DateTime.Now.AddDays(30)); // Default to 30 days
                    }
                    await _bloodBagRepository.UpdateAsync(newBloodBag);
                    _logger.LogInformation("Blood bag expiration date updated to {ExpirationDate}", newBloodBag.ExpirationDate);
                }

                // ALWAYS update global stock based on blood bag status
                var stock = await _globalStockRepository.GetByKeyAsync(newBloodBag.BloodType, newBloodBag.BloodBagType);
                if (stock == null)
                {
                    _logger.LogError("Global stock for {BloodType} {BloodBagType} not found. Please create it first.",
                        newBloodBag.BloodType.Value, newBloodBag.BloodBagType.Value);
                    return (null, new NotFoundException($"Global stock for {newBloodBag.BloodType.Value} {newBloodBag.BloodBagType.Value} not found. Please create it first.", "creating blood bag"));
                }

                // Update appropriate stock count based on blood bag status
                if (newBloodBag.Status.Value == BloodBagStatus.Ready().Value)
                {
                    // Increment ready count for ready blood bags
                    stock.IncrementAvailableCount(1);
                    _logger.LogInformation("Incremented ready count for {BloodType} {BloodBagType}",
                        newBloodBag.BloodType.Value, newBloodBag.BloodBagType.Value);
                }
                else if (newBloodBag.Status.Value == BloodBagStatus.Aquired().Value)
                {
                    // For acquired bags, also increment ready count since they're available
                    stock.IncrementAvailableCount(1);
                    _logger.LogInformation("Incremented acquired count for {BloodType} {BloodBagType}",
                        newBloodBag.BloodType.Value, newBloodBag.BloodBagType.Value);
                }
                else if (newBloodBag.Status.Value == BloodBagStatus.Expired().Value)
                {
                    // Use the specific method for incrementing expired count instead of UpdateCounts
                    stock.IncrementExpiredCount(1);
                    _logger.LogInformation("Incremented expired count for {BloodType} {BloodBagType}",
                        newBloodBag.BloodType.Value, newBloodBag.BloodBagType.Value);
                }
                else if (newBloodBag.Status.Value == BloodBagStatus.Used().Value)
                {
                    // For used bags, we don't increment ready count since they're not available
                    _logger.LogInformation("Blood bag marked as used - not affecting available counts for {BloodType} {BloodBagType}",
                        newBloodBag.BloodType.Value, newBloodBag.BloodBagType.Value);
                }
                else
                {
                    // Default case - increment available count for any other status
                    stock.IncrementAvailableCount(1);
                    _logger.LogInformation("Incremented available count for {BloodType} {BloodBagType} with status {Status}",
                        newBloodBag.BloodType.Value, newBloodBag.BloodBagType.Value, newBloodBag.Status.Value);
                }

                await _globalStockRepository.UpdateAsync(stock);

                // Publish stock update to Kafka
                var globalStockTopic = _kafkaSettings.Value.Topics["GlobalStock"];
                var subMessage = new GlobalStockData(
                    newBloodBag.BloodType,
                    newBloodBag.BloodBagType,
                    stock.ReadyCount + stock.CountExpiring + stock.CountExpired,
                    stock.ReadyCount,
                    stock.MinStock,
                    stock.CountExpired);
                var hospital = await _centerRepository.GetPrimaryAsync();
                if (hospital == null)
                {
                    throw new NotFoundException("Primary blood transfer center not found", "creating blood bag");
                }
                var message = new GlobalStockEvent(
                    hospital.Id,
                    subMessage
                );
                await _eventProducer.ProduceAsync("global-stock", message);

                return (new BloodBagDTO
                {
                    Id = newBloodBag.Id,
                    BloodBagType = newBloodBag.BloodBagType,
                    BloodType = newBloodBag.BloodType,
                    Status = newBloodBag.Status,
                    ExpirationDate = newBloodBag.ExpirationDate,
                    AcquiredDate = newBloodBag.AcquiredDate,
                    DonorId = newBloodBag.DonorId ?? throw new InvalidOperationException("DonorId cannot be null"),
                    RequestId = newBloodBag.RequestId
                }, null);
            }
            catch (BaseException ex)
            {
                _logger.LogError(ex, "Error creating blood bag");
                return (null, ex);
            }
        }
    }
}
