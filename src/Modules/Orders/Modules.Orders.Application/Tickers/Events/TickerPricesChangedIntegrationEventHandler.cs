using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Orders.Domain;
using Modules.Orders.Domain.Tickers;
using Modules.Price.IntegrationEvents;
using SharedKernel.Messaging;
using System.Threading;

namespace Modules.Orders.Application.Tickers.Events
{
    public class TickerPricesChangedIntegrationEventHandler(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<TickerPricesChangedIntegrationEventHandler> logger) : IIntegrationEventHandler<TickerPricesChangedIntegrationEvent>
    {
        public async Task Handle(TickerPricesChangedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Orders, Ticker Prices Changed: {@Tickers}", integrationEvent.Tickers);

            var updateTasks = integrationEvent.Tickers
                .Select(t => CreateOrUpdateTicker(t, cancellationToken));

            await Task.WhenAll(updateTasks);
        }

        private async Task CreateOrUpdateTicker(TickerPrice change, CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var tickerRepository = scope.ServiceProvider.GetRequiredService<ITickerRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            Ticker? ticker = await tickerRepository.GetByIdAsync(change.TickerId, cancellationToken);

            if (ticker is null)
            {
                await tickerRepository.AddAsync(Ticker.Create(change.TickerId, change.TickerCode, change.Price, DateTime.UtcNow), cancellationToken);
            }
            else
            {
                ticker.UpdatePrice(change.Price);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
