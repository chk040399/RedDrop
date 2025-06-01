namespace BD.SharedKernel;

public interface IHasDomainEvents
{
  IReadOnlyCollection<DomainEventBase> DomainEvents { get; }
  public void RegisterDomainEvent(DomainEventBase domainEvent);
  public void ClearDomainEvents();

}
