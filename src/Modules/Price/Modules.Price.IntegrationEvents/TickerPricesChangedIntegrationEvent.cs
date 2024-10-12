using SharedKernel.Messaging;

namespace Modules.Price.IntegrationEvents;


public sealed class TickerPricesChangedIntegrationEvent(TickerPrice[] tickers) : IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime OccuredOnUtc { get; }
    public TickerPrice[] Tickers { get; } = tickers;
}

public record TickerPrice(Guid TickerId, decimal Price);
