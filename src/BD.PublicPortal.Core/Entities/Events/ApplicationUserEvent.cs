using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Core.Entities.Events;

public class ApplicationUserEvent : DomainEventBase
{
  public ApplicationUserEvent(ApplicationUser applicationUser)
  {
    UserEntity = applicationUser;
    EventType = "NewOrUpdatedUser";
  }
  public ApplicationUser UserEntity  { get; set; }
  public string EventType { get; set; }

}
