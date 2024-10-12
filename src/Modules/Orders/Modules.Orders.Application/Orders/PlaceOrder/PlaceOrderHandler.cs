using FluentResults;
using Modules.Orders.Domain;
using SharedKernel.Application.Abstraction.Data;
using SharedKernel.Messaging;

namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed class PlaceOrderHandler(IOrderRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<PlaceOrder, PlaceOrderResponse>
{
    public async Task<Result<PlaceOrderResponse>> Handle(PlaceOrder request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.Ticker, request.Quantity, 0, request.UserId);

        await repository.AddOrderAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PlaceOrderResponse(order.Id.Value);
    }
}

