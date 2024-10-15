using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Orders.IntegrationEvents;
using Modules.Portfolio.Domain;
using Modules.Portfolio.Domain.Orders;
using Modules.Portfolio.Domain.Orders.Abstraction;
using Modules.Portfolio.Domain.Orders.ValueObjects;
using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Orders.OrderPlaced;

public class OrderPlacedIntegrationEventHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<OrderPlacedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderPlacedIntegrationEvent>
{
    public async Task Handle(OrderPlacedIntegrationEvent e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Portfolio, order placed: {@IntegrationEvent}", e);

        using var scope = serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var orderResult = Order.Create(e.OrderId, e.TickerId, e.Quantity, e.Price, (OrderSide)e.Side, e.UserId);

        if (orderResult.IsFailed)
        {
            logger.LogError("Order not created {@Errors}", orderResult.Errors);
            return;
        }

        await repository.AddOrderAsync(orderResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
