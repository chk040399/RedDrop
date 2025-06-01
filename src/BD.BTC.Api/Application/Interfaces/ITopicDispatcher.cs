using MediatR;
namespace  Application.Interfaces
{
    public interface ITopicDispatcher
    {
        // Add this non-generic method specifically for DonorPledgeCommand
        void RegisterDonorPledge(string topic);
        
        // Keep your existing methods
        void Register<TCommand>(string topic) where TCommand : IRequest<Unit>;
        Type? GetHandlerType(string topic);
        Type? GetMessageType(string topic);
    }
}
