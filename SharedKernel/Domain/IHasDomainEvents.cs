namespace SharedKernel.Domain;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public IDomainEvent[] PopDomainEvents();
}