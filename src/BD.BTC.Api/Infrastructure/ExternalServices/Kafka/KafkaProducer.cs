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
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonSerializer.Serialize(@event)
                };

                try
                {
                    await _producer.ProduceAsync(topic, message);
                    _logger.LogDebug($"Delivered to topic {topic}");
                }
                catch (ObjectDisposedException ex)
                {
                    _logger.LogWarning(ex, "Kafka producer was disposed. Message will not be sent to topic {Topic}", topic);
                    // Continue execution - don't let Kafka issues stop the process
                }
                catch (KafkaException kex)
                {
                    _logger.LogError(kex, "Kafka error when producing to topic {Topic}", topic);
                    // Continue execution - don't let Kafka issues stop the process
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize or send message to Kafka topic {Topic}", topic);
                // Continue execution - don't let Kafka issues stop the process
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
