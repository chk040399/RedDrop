using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BD.PublicPortal.Api.Kafka;

public class KafkaConsumerBackgroundService<TKey, TValue> : BackgroundService
{
  private readonly IConsumer<TKey, TValue> _consumer;
  private readonly ILogger<KafkaConsumerBackgroundService<TKey, TValue>> _logger;
  private readonly KafkaConsumerOptions _options;

  public KafkaConsumerBackgroundService(
    IConsumer<TKey, TValue> consumer,
    IOptions<KafkaConsumerOptions> options,
    ILogger<KafkaConsumerBackgroundService<TKey, TValue>> logger)
  {
    _consumer = consumer;
    _logger = logger;
    _options = options.Value;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    return Task.Run(() =>
    {
      _consumer.Subscribe(_options.Topics);

      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          var result = _consumer.Consume(stoppingToken);
          if (result != null)
          {
            _logger.LogInformation("Received message on topic {Topic}: {Value}", result.Topic, result.Message.Value);
          }
        }
        catch (ConsumeException ex)
        {
          _logger.LogError(ex, "Kafka consume error.");
        }
      }

      _consumer.Close();
    }, stoppingToken);
  }
}
