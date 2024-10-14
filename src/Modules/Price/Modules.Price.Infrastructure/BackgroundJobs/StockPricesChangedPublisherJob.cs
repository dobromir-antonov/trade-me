using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Price.IntegrationEvents;

namespace Modules.Price.Infrastructure.BackgroundJobs
{
    public sealed class StockPricesChangedPublisherJob(
        IPriceGenerator priceGenerator,
        IServiceScopeFactory serviceScopeFactory, 
        ILogger<StockPricesChangedPublisherJob> logger) : IHostedService
    {
        private Task _job;
        private PeriodicTimer _timer;
        private const int ExecutionPeriodInSeconds = 15;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _job = PeriodicJobAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            await _job.WaitAsync(cancellationToken);
        }

        private async Task PeriodicJobAsync(CancellationToken ct = default)
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(ExecutionPeriodInSeconds));

            while (await _timer.WaitForNextTickAsync(ct))
            {
                try
                {
                    await PublishTickerPriceChangedEventsAsync(ct);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async Task PublishTickerPriceChangedEventsAsync(CancellationToken ct = default)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var integrationEventPublisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            var tickerPrices = priceGenerator.GenerateRandomTickerPrices()
                .Select(tp => new TickerPrice(tp.TickerId, tp.TickerCode, tp.Price))
                .ToArray();

            await integrationEventPublisher.Publish(new TickerPricesChangedIntegrationEvent(tickerPrices), ct);
        }

    }
}
