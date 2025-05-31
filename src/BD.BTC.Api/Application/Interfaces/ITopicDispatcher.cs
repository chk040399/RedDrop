using MediatR;

namespace Application.Interfaces
{
    public interface ITopicDispatcher
    {
        void Register<TCommand>(string topic) where TCommand : IRequest;
        void Register<TCommand, TResult>(string topic) where TCommand : IRequest<TResult>;
        Type? GetHandlerType(string topic);
        Type? GetMessageType(string topic);
    }
}
