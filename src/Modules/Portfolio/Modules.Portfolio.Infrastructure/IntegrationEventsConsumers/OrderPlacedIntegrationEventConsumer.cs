using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Orders.IntegrationEvents;
using Modules.Portfolio.Application.Orders.PlaceOrder;

namespace Modules.Portfolio.Infrastructure.IntegrationEventConsumers;

public sealed class OrderPlacedIntegrationEventConsumer(ISender sender, ILogger<OrderPlacedIntegrationEventConsumer> logger) : IConsumer<OrderPlacedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
    {
        var m = context.Message;
        var command = new PlaceOrder(m.OrderId, m.TickerId, m.Quantity, m.Price, m.Side, m.UserId);
        await sender.Send(command, context.CancellationToken);
    }
}