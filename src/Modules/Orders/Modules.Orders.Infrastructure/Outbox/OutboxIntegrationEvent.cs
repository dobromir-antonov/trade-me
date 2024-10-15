namespace Modules.Orders.Infrastructure.Outbox;

public sealed class OutboxIntegrationEvent
{
    public required Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}
