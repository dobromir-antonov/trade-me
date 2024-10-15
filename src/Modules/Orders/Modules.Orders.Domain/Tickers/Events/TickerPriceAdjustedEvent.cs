using Modules.Orders.Domain.Tickers.ValueObjects;
using SharedKernel.Domain;

namespace Modules.Orders.Domain.Tickers;

public record TickerPriceAdjustedEvent(TickerId TickerId, decimal NewPrice) : IDomainEvent;
