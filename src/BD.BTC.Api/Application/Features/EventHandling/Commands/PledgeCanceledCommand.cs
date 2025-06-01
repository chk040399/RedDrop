using Domain.Events;
using MediatR;

namespace Application.Features.EventHandling.Commands
{
    public class PledgeCanceledCommand : IRequest<Unit>
    {
        public PledgeCanceledEvent Event { get; }

        public PledgeCanceledCommand(PledgeCanceledEvent @event)
        {
            Event = @event;
        }
    }
}
