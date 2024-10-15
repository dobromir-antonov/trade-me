using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Orders.Domain.Orders;
using Modules.Orders.Domain.Tickers;
using Modules.Orders.Infrastructure.Outbox;
using Modules.Orders.Infrastructure.Persistance;
using Modules.Orders.IntegrationEvents;
using Newtonsoft.Json;
using SharedKernel.Messaging;
using System.Text.Json;

namespace Modules.Orders.Infrastructure.OutboxWriter;


public class OutboxIntegrationEventsWriter(IServiceScopeFactory scopeFactory, ILogger<OutboxIntegrationEventsWriter> logger) :
    INotificationHandler<OrderPlacedEvent>,
    INotificationHandler<TickerCreatedEvent>
{

    private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public async Task Handle(OrderPlacedEvent e, CancellationToken cancellationToken)
    {
        IIntegrationEvent integrationEvent = new OrderPlacedIntegrationEvent(
           OrderId: e.OrderId,
           TickerId: e.TickerId, 
           Quantity: e.Quantity,
           Price: e.Price,
           Side: e.Side,
           UserId: e.UserId);

        await AddOutboxIntegrationEventAsync(integrationEvent, cancellationToken);
    }

    public Task Handle(TickerCreatedEvent e, CancellationToken cancellationToken)
    {
        logger.LogInformation("outbox writer @e", e);
        return Task.CompletedTask;
    }

    private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        //TODO: SHOULD USE THE SAME DB TRANSACTION 
        //CURRENTLY IT USES NEW TRANSACTION
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        var outboxMessage = new OutboxIntegrationEvent
        {
            Id = Guid.NewGuid(),
            Type = integrationEvent.GetType().FullName,
            Content = JsonConvert.SerializeObject(integrationEvent, _jsonSerializerSettings),
            CreatedOnUtc = DateTime.UtcNow
        };

        await dbContext.OutboxIntegrationEvents.AddAsync(outboxMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}