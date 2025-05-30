using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Events;
using Domain.Repositories;
using Domain.ValueObjects;
using Application.Features.EventHandling.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Microsoft.Extensions.Options;
using Infrastructure.Configuration;

namespace Application.Features.EventHandling.Handlers
{
    public class PledgeCanceledEventHandler : IRequestHandler<PledgeCanceledCommand,Unit>
    {
        private readonly IPledgeRepository _pledgeRepository;
        private readonly IBloodTransferCenterRepository _centerRepository;
        private readonly ILogger<PledgeCanceledEventHandler> _logger;
        private readonly RetryPolicySettings _retrySettings;
        
        public PledgeCanceledEventHandler(
            IPledgeRepository pledgeRepository,
            IBloodTransferCenterRepository centerRepository,
            ILogger<PledgeCanceledEventHandler> logger,
            IOptions<RetryPolicySettings> retrySettings)
        {
            _pledgeRepository = pledgeRepository;
            _centerRepository = centerRepository;
            _logger = logger;
            _retrySettings = retrySettings.Value;
        }
        
        public async Task<Unit> Handle(PledgeCanceledCommand command, CancellationToken cancellationToken)
        {
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    _retrySettings.MaxRetryCount,
                    attempt => TimeSpan.FromMilliseconds(
                        _retrySettings.InitialDelayMs * 
                        Math.Pow(_retrySettings.BackoffExponent, attempt - 1)),
                    onRetry: (ex, delay) => 
                    {
                        _logger.LogWarning(ex, 
                            "Retrying pledge cancellation processing in {Delay}ms", 
                            delay.TotalMilliseconds);
                    });

            try
            {
                return await policy.ExecuteAsync(async () => 
                {
                    await ProcessPledgeCancellation(command.Payload, cancellationToken);
                    return Unit.Value;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pledge cancellation processing failed after retries");
                throw;
            }
        }
        
        private async Task ProcessPledgeCancellation(PledgeCanceledEvent payload, CancellationToken cancellationToken)
        {
            // Get the hospital ID from the primary blood transfer center
            var primaryCenter = await _centerRepository.GetPrimaryAsync();
            if (primaryCenter == null)
            {
                _logger.LogError("No primary blood transfer center found");
                return;
            }
            
            var localHospitalId = primaryCenter.Id;
            
            if (payload.HospitalId != localHospitalId)
            {
                _logger.LogInformation(
                    "Ignoring pledge cancellation event - hospital ID {EventHospitalId} doesn't match local hospital ID {LocalHospitalId}",
                    payload.HospitalId,
                    localHospitalId);
                return;
            }
            
            _logger.LogInformation(
                "Processing pledge cancellation from external system for donor {DonorId} and request {RequestId}",
                payload.DonorId, 
                payload.RequestId);
            
            // Get the pledge
            var pledge = await _pledgeRepository.GetByDonorAndRequestIdAsync(
                payload.DonorId, 
                payload.RequestId);
            
            if (pledge == null)
            {
                _logger.LogWarning(
                    "Attempted to cancel non-existent pledge for donor {DonorId} and request {RequestId}",
                    payload.DonorId, 
                    payload.RequestId);
                return;
            }
            
            // Check if the event contains PledgeDate
            if (payload.PledgeDate.HasValue)
            {
                // Just update the date
                pledge.UpdatePledgeDate(DateOnly.FromDateTime(payload.PledgeDate.Value));
                _logger.LogInformation(
                    "Updated pledge date for donor {DonorId} and request {RequestId} to {NewDate}",
                    payload.DonorId,
                    payload.RequestId,
                    payload.PledgeDate.Value);
            }
            else
            {
                // No date provided, update the status to canceled if not already in terminal state
                if (!pledge.Status.IsFulfilled && !pledge.Status.IsCanceled)
                {
                    pledge.UpdateStatus(PledgeStatus.Canceled);
                    _logger.LogInformation(
                        "Pledge for donor {DonorId} and request {RequestId} canceled due to event without date",
                        payload.DonorId,
                        payload.RequestId);
                }
                else
                {
                    _logger.LogWarning(
                        "Cannot cancel pledge with status {Status} for donor {DonorId} and request {RequestId}",
                        pledge.Status.Value,
                        payload.DonorId, 
                        payload.RequestId);
                    return;
                }
            }
            
            // Save the changes
            await _pledgeRepository.UpdateAsync(pledge);
        }
    }
}
