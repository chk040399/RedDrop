using BD.Central.Core.DTOs;

namespace BD.Central.Api.Kafka.EventDTOs;

public class DonorOperationEvent
{
  public DonorOperationEvent(ApplicationUserDTO userDTO,string evtType)
  {
    ApplicationUser = userDTO;
    EventType = evtType;
  }
  public string EventType { get; init; }
  public ApplicationUserDTO ApplicationUser { get; init; }

}
