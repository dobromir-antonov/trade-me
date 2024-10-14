using Modules.Price.Domain;
using Modules.Price.Infrastructure.Persistance;
using Modules.Price.IntegrationEvents;

namespace Modules.Price.Infrastructure;

public sealed class PriceGenerator(PriceDbContext dbContext) : IPriceGenerator
{
    private const decimal MIN_PRICE = 10;
    private const decimal MAX_PRICE = 150;
   
    public TickerPrice[] GenerateRandomTickerPrices()
    {
        var rnd = new Random();
        return dbContext.Tickers
            .Select(t => new TickerPrice(
                t.Id, 
                t.Code, 
                Math.Round((decimal)(rnd.NextDouble() * (double)(MAX_PRICE - MIN_PRICE)) + MIN_PRICE, 2)))
            .ToArray();
    }
}
