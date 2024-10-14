namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed record PlaceOrderRequest(Guid UserId, Guid TickerId, int Quantity);





