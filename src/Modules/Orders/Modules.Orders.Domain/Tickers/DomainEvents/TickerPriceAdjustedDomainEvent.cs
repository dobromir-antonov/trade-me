using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public record TickerPriceAdjustedDomainEvent(TickerId TickerId, decimal NewPrice) : IDomainEvent;
