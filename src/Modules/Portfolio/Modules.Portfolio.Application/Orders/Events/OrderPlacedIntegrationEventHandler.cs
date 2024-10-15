using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Orders.IntegrationEvents;
using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Orders.Events;

public class OrderPlacedIntegrationEventHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<OrderPlacedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
{
    public async Task Handle(OrderPlacedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Portfolio, order placed: {@IntegrationEvent}", integrationEvent);
    }
}
