using Modules.Price.IntegrationEvents;

namespace Modules.Price.Infrastructure;

public interface IPriceGenerator
{
    TickerPrice[] GenerateRandomTickerPrices();
}