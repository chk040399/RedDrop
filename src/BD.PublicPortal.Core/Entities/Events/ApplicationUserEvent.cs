using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Core.Entities.Events;

public class ApplicationUserEvent : DomainEventBase
{
  public ApplicationUserEvent(ApplicationUser applicationUser)
  {
    UserEntity = applicationUser;
    EventType = "NewUser";
  }
  public ApplicationUser UserEntity  { get; set; }
  public string EventType { get; set; }

}
