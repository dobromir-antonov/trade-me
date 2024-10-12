namespace SharedKernel.Messaging;
public interface IIntegrationEventHandler<T> where T : class, IIntegrationEvent
{
    public Task Handle(T integrationEvent, CancellationToken cancellationToken);
}
