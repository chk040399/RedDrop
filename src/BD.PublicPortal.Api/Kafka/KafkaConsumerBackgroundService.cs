using BD.PublicPortal.Api.Kafka.EventDTOs;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BD.PublicPortal.Api.Kafka;

public class KafkaConsumerBackgroundService : BackgroundService
{
  private readonly IConsumer<string, string> _consumer;
  private readonly ILogger<KafkaConsumerBackgroundService> _logger;
  private readonly KafkaSettings _options;
  private readonly IServiceScopeFactory _scopeFactory;

  public KafkaConsumerBackgroundService(
      IConsumer<string, string> consumer,
      IOptions<KafkaSettings> options,
      ILogger<KafkaConsumerBackgroundService> logger,
      IServiceScopeFactory scopeFactory)
  {
    _consumer = consumer;
    _logger = logger;
    _options = options.Value;
    _scopeFactory = scopeFactory;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    return Task.Run(async () =>
    {
      _consumer.Subscribe(_options.ConsumerTopics);

      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          var result = _consumer.Consume(stoppingToken);
          if (result != null)
          {
            _logger.LogInformation("Received message on topic {Topic}: {Value}", result.Topic, result.Message.Value);
            using var scope = _scopeFactory.CreateScope();

            if (result.Topic == "cts-init")
            {
              
              var repo = scope.ServiceProvider.GetRequiredService<IRepository<BloodTansfusionCenter>>();

              var ctsData = CtsData.FromJson(result.Message.Value);
              if (ctsData != null)
              {
                var ctsDto = ctsData.ToBloodTansfusionCenterDto();
                var ctsEntity = await repo.GetByIdAsync(ctsDto.Id);
                if (ctsEntity == null)
                {
                  _logger.LogInformation("!!! KAFKA : creating BTC");
                  var created = await repo.AddAsync(ctsDto.ToEntity(), stoppingToken);
                  _logger.LogInformation($"!!! KAFKA : BTC successfully created :{created.Id} - {created.Name}");
                }
              }
              else
              {
                _logger.LogError($"!!! KAFKA : creating BTC, Can't deserialize: {result.Message.Value}");
              }
            }
            else if (result.Topic == "blood-request-created")
            {
              var repo = scope.ServiceProvider.GetRequiredService<IRepository<BloodDonationRequest>>();
              var rce = RequestCreatedEvent.FromJson(result.Message.Value);
              if (rce != null)
              {
                var reqDto = rce.ToDto();
                var ctsEntity = await repo.GetByIdAsync(reqDto.Id);
                if (ctsEntity == null)
                {
                  _logger.LogInformation("!!! KAFKA : creating Blood request");
                  var created = await repo.AddAsync(reqDto.ToEntity(), stoppingToken);
                  _logger.LogInformation($"!!! KAFKA : Request created :{created.Id}");
                }
              }
              else
              {
                _logger.LogError($"!!! KAFKA : creating BTC, Can't deserialize: {result.Message.Value}");
              }

            }
            else if (result.Topic == "update-request")
            {

            }
            else if (result.Topic == "pledge-canceled-events")
            {

            }
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
