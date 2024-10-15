using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Orders.Infrastructure.Persistance;
using Newtonsoft.Json;
using SharedKernel.Messaging;
using System.Reflection;

namespace Modules.Orders.Infrastructure.Outbox;

public sealed class OutboxIntegrationEventsPublisherJob(
    IServiceScopeFactory serviceScopeFactory, 
    ILogger<OutboxIntegrationEventsPublisherJob> logger) : IHostedService
{
    private Task _job;
    private PeriodicTimer _timer;
    private const int ExecutionPeriodInSeconds = 5;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger<OutboxIntegrationEventsPublisherJob> _logger = logger;
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };


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

        //TODO: scale it with FOR UPDATE SKIP LOCKED
        OutboxIntegrationEvent[] outboxIntegrationEvents = await dbContext.OutboxIntegrationEvents
            .Where(x => x.ProcessedOnUtc == null)
            .Take(10)
            .ToArrayAsync(ct);

        if (outboxIntegrationEvents.Length == 0)
        {
            return;
        }

        foreach (var e in outboxIntegrationEvents)
        {
            try
            {
                var integrationEvent = JsonConvert.DeserializeObject(e.Content, _jsonSerializerSettings);

                //TODO: Add polly for retry
                await integrationEventPublisher.Publish(integrationEvent, ct);

                e.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                e.ProcessedOnUtc = DateTime.UtcNow;
                e.Error = ex.ToString();
            }
        }

        dbContext.UpdateRange(outboxIntegrationEvents);
        await dbContext.SaveChangesAsync(ct);
    }

    private IIntegrationEvent IntegrationEventFactory(OutboxIntegrationEvent outbox)
    {
        // if (outbox.EventName == typeof(UserCreatedIntegrationEvent).Name) return JsonConvert.DeserializeObject<UserCreatedIntegrationEvent>(outbox.EventContent)!;

        throw new ArgumentException("Not supported integration event");
    }
}
