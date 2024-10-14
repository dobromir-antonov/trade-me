using FluentResults;
using Modules.Orders.Domain;
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
        var order = Order.Create(request.TickerId, request.Quantity, tickerPrice, request.UserId);

        await orderRepository.AddOrderAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PlaceOrderResponse(order.Id.Value);
    }
}

