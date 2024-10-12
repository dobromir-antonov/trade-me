using SharedKernel.Messaging;

namespace Modules.Price.IntegrationEvents;

public record TickerPriceChanged(string Ticker, decimal Price);
