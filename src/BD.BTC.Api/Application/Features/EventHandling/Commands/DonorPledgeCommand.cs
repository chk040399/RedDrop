using MediatR;
using Domain.Events;
namespace Application.Features.EventHandling.Commands
{
    public class DonorPledgeCommand : IRequest
    {
        public DonorPledgeEvent Payload { get; }

        public DonorPledgeCommand(DonorPledgeEvent payload)
        {
            Payload = payload;
        }
    }
}