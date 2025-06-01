using Domain.Repositories;
using Domain.Events;
using Infrastructure.ExternalServices.Kafka;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using Application.DTOs;
using Application.Features.BloodRequests.Commands;
using Shared.Exceptions;
using MediatR;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Domain.ValueObjects;
using BD.BTC.Api.Converters;
using Domain.Entities;
using BD.PublicPortal.Core.Entities.Enums;

namespace Application.Features.BloodRequests.Handlers
{
    // Fix: Change IRequest to IRequestHandler with proper generic parameters
    public class UpdateRequestHandler : IRequestHandler<UpdateRequestCommand, (RequestDto? request, BaseException? err)>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IEventProducer _eventProducer;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly ILogger<UpdateRequestHandler> _logger;
        private readonly IBloodTransferCenterRepository _centerRepository; // Add this if needed

        public UpdateRequestHandler(IRequestRepository requestRepository,
                                   ILogger<UpdateRequestHandler> logger,
                                   IEventProducer eventProducer,
                                   IOptions<KafkaSettings> kafkaSettings, IBloodTransferCenterRepository centerRepository)
        {
            _eventProducer = eventProducer;
            _centerRepository = centerRepository; // Initialize this if needed
            _kafkaSettings = kafkaSettings;

            _requestRepository = requestRepository;
            _logger = logger;
        }

        // The rest of your handler implementation remains the same
        public async Task<(RequestDto? request, BaseException? err)> Handle(UpdateRequestCommand command, CancellationToken ct)
        {
            try
            {
                // Validation logic remains the same
                if (command.Id == Guid.Empty)
                {
                    _logger.LogError("Invalid request ID: empty GUID");
                    return (null, new BadRequestException("Invalid request ID", "updating request"));
                }

                var request = await _requestRepository.GetByIdAsync(command.Id);
                if(request == null)
                {
                    _logger.LogError("Request with ID {RequestId} not found", command.Id);
                    return (null, new NotFoundException($"Request {command.Id} not found", "updating request"));
                }
                
                // Track if any changes were made
                bool wasModified = false;
                
                // First apply explicit status update if provided
                if (command.Status != null)
                {
                    wasModified = true;
                    // Directly set status if explicitly provided
                    if (command.Status.Value == RequestStatus.Partial().Value)
                        request.MarkAsPartial();
                    else if (command.Status.Value == RequestStatus.Resolved().Value)
                        request.Resolve();
                    else if (command.Status.Value == RequestStatus.Cancelled().Value)
                        request.Cancel();
                    else if (command.Status.Value == RequestStatus.Rejected().Value)
                        request.Reject();
                    else if (command.Status.Value == RequestStatus.Pending().Value)
                        request.MarkAsPending();
                }
                
                // Then update other details (which might override status based on quantities)
                if (command.BloodBagType != null || command.Priority != null || 
                    command.DueDate != null || command.MoreDetails != null || 
                    command.RequiredQty.HasValue)
                {
                    wasModified = true;
                    request.UpdateDetails(command.BloodBagType, command.Priority, command.DueDate, 
                        command.MoreDetails, command.RequiredQty);
                }
                
                // Update acquired quantity if provided
                if (command.AquiredQty.HasValue)
                {
                    wasModified = true;
                    // Only update with quantities if no explicit status was provided
                    if (command.Status == null)
                        request.UpdateAcquiredQuantity(command.AquiredQty.Value);
                    else
                        request.SetAcquiredQuantity(command.AquiredQty.Value); // Use new method instead
                }
                
                // Only update in database and publish event if changes were made
                if (wasModified)
                {
                    await _requestRepository.UpdateAsync(request);
                    
                    // Publish event for any change
                    var center = await _centerRepository.GetPrimaryAsync();
                    if (center != null)
                    {
                        // Convert status and priority to enums for the event
                        BloodDonationRequestEvolutionStatus? statusEnum = 
                            RequestStatusConverter.ToEnum(request.Status);
                        BloodDonationRequestPriority priorityEnum = 
                            PriorityConverter.ToEnum(request.Priority);
                        
                        var topic = _kafkaSettings.Value.Topics["UpdateRequest"];
                        var updateRequestEvent = new UpdateRequestEvent(
                            center.Id,
                            request.Id,
                            request.RequiredQty,
                            null,                // string? priority parameter
                            statusEnum,          // BloodDonationRequestEvolutionStatus? status parameter 
                            request.AquiredQty,
                            priorityEnum,        // BloodDonationRequestPriority? priorityEnum parameter
                            request.DueDate
                        );
                        
                        await _eventProducer.ProduceAsync(topic, updateRequestEvent);
                        _logger.LogInformation("Request updated and event published successfully");
                    }
                    else
                    {
                        _logger.LogWarning("Request updated but event not published: no primary blood transfer center found");
                    }
                }
                else
                {
                    _logger.LogInformation("No changes detected for request {RequestId}, skipping update", command.Id);
                }
                
                // Return DTO
                var requestDto = new RequestDto
                {
                    Id = request.Id,
                    Priority = request.Priority.Value,
                    BloodType = request.BloodType.Value,
                    BloodBagType = request.BloodBagType.Value,
                    RequestDate = request.RequestDate,
                    DueDate = request.DueDate,
                    Status = request.Status.Value,
                    MoreDetails = request.MoreDetails,
                    RequiredQty = request.RequiredQty,
                    AquiredQty = request.AquiredQty,
                    ServiceId = request.ServiceId,
                    DonorId = request.DonorId
                };
                return (requestDto, null);
            }
            catch(BaseException ex)
            {
                _logger.LogError(ex, "Error while updating request {RequestId}", command.Id);
                return (null, ex);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating request {RequestId}", command.Id);
                return (null, new InternalServerException("Failed to update request", "updating request"));
            }
        }
    }
}