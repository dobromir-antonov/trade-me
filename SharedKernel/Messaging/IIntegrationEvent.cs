namespace SharedKernel.Messaging;

public interface IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime OccuredOnUtc { get; }
}
