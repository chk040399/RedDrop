using Application.Interfaces;
using MediatR;
// Infrastructure/ExternalServices/Kafka/TopicDispatcher.cs
namespace Infrastructure.ExternalServices
{public class TopicDispatcher : ITopicDispatcher
{
    private readonly Dictionary<string, Type> _handlerRegistry = new();
    private readonly Dictionary<string, Type> _messageRegistry = new();

    /// <inheritdoc/>
    void ITopicDispatcher.Register<TCommand>(string topic)
    {
        var commandType = typeof(TCommand);
        var messageType = commandType
            .GetConstructors()
            .First()
            .GetParameters()
            .First()
            .ParameterType;

        _handlerRegistry[topic] = commandType;
        _messageRegistry[topic] = messageType;
    }

    public void RegisterDonorPledge(string topic)
    {
        var commandType = typeof(Application.Features.EventHandling.Commands.DonorPledgeCommand);
        var messageType = typeof(Domain.Events.DonorPledgeEvent);

        _handlerRegistry[topic] = commandType;
        _messageRegistry[topic] = messageType;
    }

    public void RegisterPledgeCanceled(string topic)
    {
        var commandType = typeof(Application.Features.EventHandling.Commands.PledgeCanceledCommand);
        var messageType = typeof(Domain.Events.PledgeCanceledEvent);

        _handlerRegistry[topic] = commandType;
        _messageRegistry[topic] = messageType;
    }

    public Type? GetHandlerType(string topic) => 
        _handlerRegistry.TryGetValue(topic, out var type) ? type : null;

    public Type? GetMessageType(string topic) => 
        _messageRegistry.TryGetValue(topic, out var type) ? type : null;
}}
