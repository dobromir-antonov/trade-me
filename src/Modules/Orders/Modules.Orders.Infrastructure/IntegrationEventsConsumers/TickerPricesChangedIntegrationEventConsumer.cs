using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Orders.Application.Tickers;
using Modules.Price.IntegrationEvents;

namespace Modules.Orders.Infrastructure.IntegrationEventConsumers;

public sealed class TickerPricesChangedIntegrationEventConsumer(
    ISender sender, 
    ILogger<TickerPricesChangedIntegrationEventConsumer> logger) : IConsumer<TickerPricesChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<TickerPricesChangedIntegrationEvent> context)
    {
        foreach (var change in context.Message.Tickers)
        {
            await sender.Send(new AdjustPrice(change.TickerId, change.TickerCode, change.Price), context.CancellationToken);
        }
    }
}