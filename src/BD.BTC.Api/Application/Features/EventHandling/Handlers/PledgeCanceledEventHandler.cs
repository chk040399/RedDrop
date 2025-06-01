using Application.Features.EventHandling.Commands;
using BD.BTC.Api.Converters;
using BD.PublicPortal.Core.Entities.Enums;
using Domain.Events;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EventHandling.Handlers
{
    public class PledgeCanceledEventHandler 
        : IRequestHandler<PledgeCanceledCommand, Unit>
    {
        private readonly IPledgeRepository _pledgeRepository;
        private readonly ILogger<PledgeCanceledEventHandler> _logger;

        public PledgeCanceledEventHandler(
            IPledgeRepository pledgeRepository,
            ILogger<PledgeCanceledEventHandler> logger)
        {
            _pledgeRepository = pledgeRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(PledgeCanceledCommand command, CancellationToken cancellationToken)
        {
            await ProcessPledgeCancellation(command.Event, cancellationToken);
            return Unit.Value;
        }

        private async Task ProcessPledgeCancellation(PledgeCanceledEvent payload, CancellationToken ct)
        {
            _logger.LogInformation("Processing pledge cancellation for DonorId: {DonorId}, RequestId: {RequestId}", 
                payload.DonorId, payload.RequestId);

            // Find existing pledge
            var pledge = await _pledgeRepository.GetByDonorAndRequestIdAsync(payload.DonorId, payload.RequestId);
            
            if (pledge == null)
            {
                _logger.LogWarning("No pledge found for DonorId: {DonorId} and RequestId: {RequestId}", 
                    payload.DonorId, payload.RequestId);
                return;
            }

            // Choose action based on whether PledgeDate is provided
            if (payload.PledgeDate.HasValue)
            {
                // If PledgeDate is provided, only update the date
                _logger.LogInformation("Updating pledge date to: {PledgeDate}", payload.PledgeDate.Value);
                pledge.PledgeDate = new DateTime(payload.PledgeDate.Value.Year, payload.PledgeDate.Value.Month, payload.PledgeDate.Value.Day);
            }
            else
            {
                // If no PledgeDate is provided, only cancel the pledge
                _logger.LogInformation("Cancelling pledge status");
                
                // Convert enum to value object using the converter
                var cancelledStatus = PledgeStatusConverter.ToPledgeStatus(BloodDonationPladgeEvolutionStatus.CanceledByInitiaor);
                
                // Update the pledge status only
                pledge.UpdateStatus(cancelledStatus);
            }
            
            _logger.LogInformation("Updating pledge: {Pledge}", pledge);
            
            try
            {
                await _pledgeRepository.UpdateAsync(pledge);
                _logger.LogInformation("Pledge successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pledge: {Message}", ex.Message);
                throw;
            }
        }
    }
}
