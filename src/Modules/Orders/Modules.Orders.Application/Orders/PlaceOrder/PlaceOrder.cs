using SharedKernel.Messaging;

namespace Modules.Orders.Application.Orders.PlaceOrder;

public sealed record PlaceOrder(Guid UserId, Guid TickerId, int Quantity, int Side) : ICommand<PlaceOrderResponse>;





