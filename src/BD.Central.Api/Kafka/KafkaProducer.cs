// Infrastructure/MessageBrokers/KafkaEventPublisher.cs
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BD.Central.Api.Kafka;

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
              string serializedMessage;
              if (@event is string eventStr)
              {
                  serializedMessage = eventStr;
              }
              else
              {
                  serializedMessage = JsonSerializer.Serialize(@event);
              }
              
              var message = new Message<string, string>
              {
                  Key = Guid.NewGuid().ToString(),
                  Value = serializedMessage
              };
              
              var result = await _producer.ProduceAsync(topic, message);
              _logger.LogDebug("Delivered to {Topic} [{Partition}] @{Offset}", 
                  result.Topic, result.Partition, result.Offset);
          }
          catch (ObjectDisposedException ex)
          {
              _logger.LogError(ex, "Kafka producer has been disposed");
              // Rethrow to maintain error propagation
              throw;
          }
          catch (Exception ex)
          {
              _logger.LogError(ex, "Message production failed");
              // Rethrow to maintain error propagation  
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
