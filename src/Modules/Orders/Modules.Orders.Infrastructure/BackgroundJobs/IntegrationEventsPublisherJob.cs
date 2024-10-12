using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure.Persistance;
using SharedKernel.Messaging;

namespace Modules.Orders.Infrastructure.BackgroundJobs;

public sealed class IntegrationEventsPublisherJob(IServiceScopeFactory serviceScopeFactory, ILogger<IntegrationEventsPublisherJob> logger) : IHostedService
{
    private Task _job;
    private PeriodicTimer _timer;
    private const int ExecutionPeriodInSeconds = 5;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger<IntegrationEventsPublisherJob> _logger = logger;

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
                await PublishOutboxIntegrationEventsAsync(ct);
            }
            catch (Exception ex)
            {

            }
        }
    }

    private async Task PublishOutboxIntegrationEventsAsync(CancellationToken ct = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        var integrationEventPublisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        //List<OutboxIntegrationEvent> outboxIntegrationEvents = await dbContext.OutboxIntegrationEvents.ToListAsync(ct);

        //if (outboxIntegrationEvents.Count == 0)
        //{
        //    return;
        //}

        //foreach (OutboxIntegrationEvent outboxIntegrationEvent in outboxIntegrationEvents)
        //{
        //    IIntegrationEvent integrationEvent = IntegrationEventFactory(outboxIntegrationEvent);

        //    //TODO: Add polly for retry
        //    await integrationEventPublisher.Publish(integrationEvent, integrationEvent.GetType(), ct);
        //}

        //dbContext.RemoveRange(outboxIntegrationEvents);
        await dbContext.SaveChangesAsync(ct);
    }

    private IIntegrationEvent IntegrationEventFactory(OutboxIntegrationEvent outbox)
    {
        // if (outbox.EventName == typeof(UserCreatedIntegrationEvent).Name) return JsonConvert.DeserializeObject<UserCreatedIntegrationEvent>(outbox.EventContent)!;

        throw new ArgumentException("Not supported integration event");
    }
}
