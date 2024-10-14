using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public record TickerCreatedDomainEvent(TickerId TickerId) : IDomainEvent;
