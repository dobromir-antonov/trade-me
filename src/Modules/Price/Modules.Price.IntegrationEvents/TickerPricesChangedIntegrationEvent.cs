using SharedKernel.Messaging;

namespace Modules.Price.IntegrationEvents;


public sealed record TickerPricesChangedIntegrationEvent(TickerPrice[] Tickers) : IIntegrationEvent;

public sealed record TickerPrice(Guid TickerId, string TickerCode, decimal Price);
