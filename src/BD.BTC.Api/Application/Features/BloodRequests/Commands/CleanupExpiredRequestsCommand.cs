using MediatR;
namespace Application.Features.BloodRequests.Commands
{
    public class CleanupExpiredRequestsCommand : IRequest<Unit>
    {
        // Command properties can be added here if needed
    }
}
