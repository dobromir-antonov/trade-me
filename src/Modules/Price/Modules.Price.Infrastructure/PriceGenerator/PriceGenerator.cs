using Modules.Price.Domain;
using Modules.Price.IntegrationEvents;

namespace Modules.Price.Infrastructure;

public sealed class PriceGenerator : IPriceGenerator
{
    private const decimal MIN_PRICE = 10;
    private const decimal MAX_PRICE = 150;
    private static readonly Ticker[] Tickers = [
        Ticker.Create("APPL", "Apple"),
        Ticker.Create("TSLA", "Tesla"),
        Ticker.Create("NVDA", "Nvidia")
    ];

    public TickerPrice[] GenerateRandomTickerPrices()
    {
        var rnd = new Random();
        return Tickers
            .Select(t => new TickerPrice(t.Id, Math.Round((decimal)(rnd.NextDouble() * (double)(MAX_PRICE - MIN_PRICE)) + MIN_PRICE, 2)))
            .ToArray();
    }
}
