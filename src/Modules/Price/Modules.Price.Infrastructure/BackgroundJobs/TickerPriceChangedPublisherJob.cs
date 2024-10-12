using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Price.Infrastructure.Persistance;
using Modules.Price.IntegrationEvents;
using SharedKernel.Messaging;

namespace Modules.Price.Infrastructure.BackgroundJobs
{
    public sealed class TickerPriceChangedPublisherJob(IServiceScopeFactory serviceScopeFactory, ILogger<TickerPriceChangedPublisherJob> logger) : IHostedService
    {
        private Task _job;
        private PeriodicTimer _timer;
        private const int ExecutionPeriodInSeconds = 1;

        private static readonly string[] Tickers = ["AAPL", "TSLA", "NVDA"];

        private const decimal MIN_PRICE = 10;
        private const decimal MAX_PRICE = 150;

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
            var rnd = new Random();
            var index = rnd.Next(0, Tickers.Length - 1);
            var ticker = Tickers[index];
            var price = Math.Round((decimal)(rnd.NextDouble() * (double)(MAX_PRICE - MIN_PRICE)) + MIN_PRICE, 2);

            await integrationEventPublisher.Publish(new TickerPriceChanged(ticker, price), ct);
        }



    }
}
