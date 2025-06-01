using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

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
