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

namespace Application.Features.BloodRequests.Handlers
{
    // Fix: Change IRequest to IRequestHandler with proper generic parameters
    public class UpdateRequestHandler : IRequestHandler<UpdateRequestCommand, (RequestDto? request, BaseException? err)>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IEventProducer _eventProducer;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly ILogger<UpdateRequestHandler> _logger;

        public UpdateRequestHandler(IRequestRepository requestRepository, 
                                   ILogger<UpdateRequestHandler> logger, 
                                   IEventProducer eventProducer, 
                                   IOptions<KafkaSettings> kafkaSettings)
        {
            _eventProducer = eventProducer;
            _kafkaSettings = kafkaSettings;
        
            _requestRepository = requestRepository;
            _logger = logger;
        }

        // The rest of your handler implementation remains the same
        public async Task<(RequestDto? request, BaseException? err)> Handle(UpdateRequestCommand command, CancellationToken ct)
        {
            try
            {
                // Add validation for empty GUID
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
                
                if (command.DueDate != null)
                {
                    var topic = _kafkaSettings.Value.Topics["UpdateRequest"];
                    var updateRequestEvent = new UpdateRequestEvent(
                        command.Id,
                        command.Priority?.Value,  // Add null check with ? operator
                        null,
                        request.AquiredQty,
                        request.RequiredQty,
                        command.DueDate
                    );
                    var message = updateRequestEvent;
                    await _eventProducer.ProduceAsync(topic, message);
                    _logger.LogInformation("request updated successfully");
                }
                
                // First apply explicit status update if provided
                if (command.Status != null)
                {
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
                request.UpdateDetails(command.BloodBagType, command.Priority, command.DueDate, command.MoreDetails, command.RequiredQty);
                
                // Update acquired quantity if provided
                if (command.AquiredQty.HasValue)
                {
                    // Only update with quantities if no explicit status was provided
                    if (command.Status == null)
                        request.UpdateAcquiredQuantity(command.AquiredQty.Value);
                    else
                        request.SetAcquiredQuantity(command.AquiredQty.Value); // Use new method instead
                }
                
                await _requestRepository.UpdateAsync(request);
                _logger.LogInformation("request updated successfully");
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
                return(requestDto,null);

            }catch(BaseException ex)
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