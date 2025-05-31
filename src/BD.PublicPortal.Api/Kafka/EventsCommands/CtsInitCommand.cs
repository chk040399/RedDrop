
namespace BD.PublicPortal.Api.Kafka.EventsCommands;

  public class CtsInitCommand : IRequest<Unit>
  {
      public CtsInitCommand Payload { get; }

      public CtsInitCommand(CtsInitCommand payload)
      {
          Payload = payload;
      }
  }
