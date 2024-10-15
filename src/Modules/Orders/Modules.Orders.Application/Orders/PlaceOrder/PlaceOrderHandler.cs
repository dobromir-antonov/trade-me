using FluentResults;
using Modules.Orders.Domain;
using Modules.Orders.Domain.Orders.Abstraction;
using Modules.Orders.Domain.Tickers.Abstraction;
using Modules.Orders.Domain.ValueObjects;
using SharedKernel.Messaging;

namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed class PlaceOrderHandler(
    IOrderRepository orderRepository, 
    ITickerRepository tickerRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<PlaceOrder, PlaceOrderResponse>
{
    public async Task<Result<PlaceOrderResponse>> Handle(PlaceOrder request, CancellationToken cancellationToken)
    {
        decimal tickerPrice = await tickerRepository.GetPriceByIdAsync(request.TickerId, cancellationToken);
        Result<Order> orderResult = Order.Create(
            request.TickerId, 
            request.Quantity, 
            tickerPrice, 
            (OrderSide)request.Side, 
            request.UserId);

        if (orderResult.IsFailed)
        {
            return Result.Fail(orderResult.Errors);
        }

        Order order = orderResult.Value;

        await orderRepository.AddOrderAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PlaceOrderResponse(order.Id.Value);
    }
}

