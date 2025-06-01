using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Application.DTOs;
using Infrastructure.ExternalServices.Kafka;
using Application.Features.BloodRequests.Commands;
using Microsoft.Extensions.Options;
using Domain.Events;
using Shared.Exceptions;
using Application.Interfaces;
using BD.BTC.Api.Converters;

namespace Application.Features.BloodRequests.Handlers
{
    public class CreateRequestHandler : IRequestHandler<CreateRequestCommand, RequestDto>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IEventProducer _eventProducer;
        private readonly IServiceRepository _serviceRepository;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly ILogger<CreateRequestHandler> _logger;
        private readonly IBloodTransferCenterRepository _centerRepository; // Add this

        public CreateRequestHandler(
            IRequestRepository requestRepository,
            ILogger<CreateRequestHandler> logger,
            IEventProducer eventProducer,
            IOptions<KafkaSettings> kafkaSettings,
            IServiceRepository serviceRepository,
            IBloodTransferCenterRepository centerRepository) // Add this parameter
        {
            _serviceRepository = serviceRepository;
            _eventProducer = eventProducer;
            _kafkaSettings = kafkaSettings;
            _requestRepository = requestRepository;
            _logger = logger;
            _centerRepository = centerRepository; // Initialize this
        }

        public async Task<RequestDto> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a new request
                var newRequest = new Request(
                    request.BloodType,
                    request.Priority,
                    request.BloodBagType,
                    request.RequestDate,
                    request.DueDate,
                    request.RequestStatus,
                    request.MoreDetails,
                    request.RequiredQty,
                    request.AquiredQty,
                    request.ServiceId,
                    request.DonorId);
                
                _logger.LogInformation("Creating new blood request with ID: {RequestId}", newRequest.Id);
                
                // Save the request to the database
                await _requestRepository.AddAsync(newRequest);

                // Check if the service exists
                if (!newRequest.ServiceId.HasValue)
                {
                    _logger.LogError("ServiceId is null");
                    throw new ArgumentException("ServiceId cannot be null", nameof(request.ServiceId));
                }

                var service = await _serviceRepository.GetByIdAsync(newRequest.ServiceId.Value);
                if (service == null)
                {
                    _logger.LogError("Service not found");
                    throw new NotFoundException("Service not found", "CreateRequestHandler");
                }
                
                // Fetch blood transfer center data
                var bloodCenter = await _centerRepository.GetPrimaryAsync();
                if (bloodCenter == null)
                {
                    _logger.LogWarning("Blood transfer center not found, using empty GUID for hospital ID");
                    // Continue with empty GUID if no center is found
                }

                // Create the Kafka event
                var topic = _kafkaSettings.Value.Topics["BloodRequests"];
                var message = new RequestCreatedEvent(
                    bloodCenter?.Id ?? Guid.Empty, // Use center ID or empty GUID if not found
                    newRequest.Id,
                    BloodGroupConverter.ToEnum(newRequest.BloodType),
                    PriorityConverter.ToEnum(newRequest.Priority),
                    BloodBagTypeConverter.ToEnum(newRequest.BloodBagType),
                    newRequest.RequestDate,
                    newRequest.DueDate,
                    RequestStatusConverter.ToEnum(newRequest.Status),
                    newRequest.MoreDetails,
                    newRequest.RequiredQty,
                    newRequest.AquiredQty,
                    service.Name);
                
                // Publish to Kafka
                try
                {
                    await _eventProducer.ProduceAsync(topic, message);
                    _logger.LogInformation("Published blood request creation event to Kafka for request ID: {RequestId}", newRequest.Id);
                }
                catch (Exception ex)
                {
                    // Log but don't fail the request creation if Kafka publishing fails
                    _logger.LogError(ex, "Failed to publish blood request creation event to Kafka for request ID: {RequestId}", newRequest.Id);
                }

                // Return the DTO
                return new RequestDto
                {
                    Id = newRequest.Id,
                    Priority = newRequest.Priority.Value,
                    BloodType = newRequest.BloodType.Value,
                    BloodBagType = newRequest.BloodBagType.Value,
                    RequestDate = newRequest.RequestDate,
                    DueDate = newRequest.DueDate,
                    Status = newRequest.Status.Value,
                    MoreDetails = newRequest.MoreDetails,
                    RequiredQty = newRequest.RequiredQty,
                    AquiredQty = newRequest.AquiredQty,
                    ServiceId = newRequest.ServiceId,
                    DonorId = newRequest.DonorId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blood request");
                throw;
            }
        }
    }
}