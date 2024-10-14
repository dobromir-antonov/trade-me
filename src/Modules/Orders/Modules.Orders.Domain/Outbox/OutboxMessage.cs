namespace Modules.Orders.Domain.Outbox;

public sealed class OutboxMessage(string EventName, string EventContent, DateTime CreatedOn)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string EventName { get; private set; } = EventName;
    public string EventContent { get; private set; } = EventContent;
    public DateTime CreatedOn { get; private set; } = CreatedOn;
}
