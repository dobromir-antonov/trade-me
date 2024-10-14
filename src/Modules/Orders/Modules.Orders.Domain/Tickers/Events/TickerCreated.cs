using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public record TickerCreated(TickerId TickerId) : IDomainEvent;
