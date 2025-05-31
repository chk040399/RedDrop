using Ardalis.GuardClauses;
using BD.PublicPortal.Api.Kafka;
using BD.PublicPortal.Api.Kafka.Events;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Events;
using Microsoft.Extensions.Options;


namespace BD.PublicPortal.Api.EventsHandlers;

public record DonorOperationKafKaEvent(string EvtType, Guid UserId,ApplicationUserDTO UserDTO);

public class ApplicationUserEventHandler:INotificationHandler<ApplicationUserEvent>
{
  public ApplicationUserEventHandler(IEventProducer eventProducer, IOptions<KafkaSettings> kafkaSettings, ILogger<ApplicationUserEventHandler> logger)
  {
    _logger = logger;
    _eventProducer = eventProducer;
    _kafkaSettings = kafkaSettings;
  }
  public async Task Handle(ApplicationUserEvent domainEvent, CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent, nameof(domainEvent));
    _logger.LogInformation($"!!! DDD Event received a new/updated Donor {domainEvent.UserEntity.Id} - {domainEvent.UserEntity.DonorName} registered !");

    //TODO : Add welcome email !!!

    // Kafka event
    var topic = _kafkaSettings.Value.Topics["Donor"];
    Guard.Against.NullOrEmpty(topic);

    ;



    _logger.LogInformation($"!!! KAFKA : Trying to publish New/updated Donor Event {domainEvent.UserEntity.Id} - {domainEvent.UserEntity.DonorName}");
    await _eventProducer.ProduceAsync(topic, new DonorOperationKafKaEvent(domainEvent.EventType, domainEvent.UserEntity.Id, domainEvent.UserEntity.ToDto()));
    _logger.LogInformation($"!!! KAFKA : Succesfully published New/updated Donor Event {domainEvent.UserEntity.Id} - {domainEvent.UserEntity.DonorName}");
  }

  private ILogger<ApplicationUserEventHandler> _logger;
  private IEventProducer _eventProducer;
  private readonly IOptions<KafkaSettings> _kafkaSettings;
}
