using SharedKernel.Messaging;

namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed record PlaceOrder(Guid UserId, Guid TickerId, int Quantity) : ICommand<PlaceOrderResponse>;





