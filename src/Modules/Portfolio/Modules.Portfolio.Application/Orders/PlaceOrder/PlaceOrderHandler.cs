using FluentResults;
using Microsoft.Extensions.Logging;
using Modules.Portfolio.Domain;
using Modules.Portfolio.Domain.Orders;
using Modules.Portfolio.Domain.Orders.Abstraction;
using Modules.Portfolio.Domain.Orders.ValueObjects;
using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Orders.PlaceOrder;

internal sealed class PlaceOrderHandler(
    IOrderRepository repository, 
    IUnitOfWork unitOfWork,
    ILogger<PlaceOrderHandler> logger) : ICommandHandler<PlaceOrder>
{
    public async Task<Result> Handle(PlaceOrder command, CancellationToken cancellationToken)
    {
        var orderResult = Order.Create(
            command.OrderId, 
            command.TickerId, 
            command.Quantity, 
            command.Price, 
            (OrderSide)command.Side,
            command.UserId);

        if (orderResult.IsFailed)
        {
            logger.LogError("Order not created {@Errors}", orderResult.Errors);
            return Result.Fail(orderResult.Errors.First());
        }

        logger.LogInformation("Order Placed {@Order}", orderResult.Value);

        await repository.AddOrderAsync(orderResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

