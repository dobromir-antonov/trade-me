using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public record TickerPriceAdjusted(TickerId TickerId, decimal NewPrice) : IDomainEvent;
