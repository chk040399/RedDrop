// Infrastructure/MessageBrokers/KafkaEventPublisher.cs
using System.Text.Json;
using Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices.Kafka
{
    public class KafkaEventPublisher : IEventProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaEventPublisher> _logger;

        public KafkaEventPublisher(
            ILogger<KafkaEventPublisher> logger,
            IOptions<KafkaSettings> settings)
        {
            _logger = logger;
            
            var kafkaSettings = settings.Value;
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                EnableIdempotence = kafkaSettings.EnableIdempotence,
                MessageTimeoutMs = 5000,
                Acks = Acks.All
            };

            _producer = new ProducerBuilder<Null, string>(config)
                .SetErrorHandler((_, e) => _logger.LogError($"Producer error: {e.Reason}"))
                .Build();
        }

        public async Task ProduceAsync<TEvent>(string topic, TEvent @event) where TEvent : class
        {
            try
            {
                var message = new Message<Null, string>
                {
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