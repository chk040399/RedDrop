// Infrastructure/MessageBrokers/KafkaEventPublisher.cs
using System.Text.Json;
using Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices.Kafka
{
    public class KafkaEventPublisher : IEventProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaEventPublisher> _logger;

        public KafkaEventPublisher(
            ILogger<KafkaEventPublisher> logger, IProducer<string, string> producer)
        {
          _producer = producer;
          _logger = logger;
        }

        public async Task ProduceAsync<TEvent>(string topic, TEvent @event) where TEvent : class
        {
            try
            {
                var message = new Message<string, string>
                {
                    Key = "CtsCreated",
                    Value = JsonSerializer.Serialize(@event)
                };

                var result = await _producer.ProduceAsync(topic, message);
                _logger.LogDebug($"Delivered to {result.TopicPartitionOffset}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message production failed");
                throw;
            }
        }

        public Task FlushAsync(TimeSpan timeout)
        {
            _producer.Flush(timeout);
            return Task.CompletedTask;
        }

        public void Dispose() => _producer.Dispose();
    }
}
