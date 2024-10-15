using Microsoft.AspNetCore.Mvc;

namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed record PlaceOrderRequest(Guid TickerId, int Quantity, int Side);





