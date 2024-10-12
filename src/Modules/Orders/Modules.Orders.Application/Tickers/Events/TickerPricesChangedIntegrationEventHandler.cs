using Microsoft.Extensions.Logging;
using Modules.Price.IntegrationEvents;
using SharedKernel.Messaging;

namespace Modules.Orders.Application.Tickers.Events
{
    public class TickerPricesChangedIntegrationEventHandler(ILogger<TickerPricesChangedIntegrationEventHandler> logger) : IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>
    {
        public async Task Handle(TickerPricesChangedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Orders, Ticker Prices Changed: {@Tickers}", notification.Tickers);
        }
    }
}
