using SharedKernel.Messaging;

namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed record PlaceOrder(string Ticker, int Quantity, Guid UserId) : ICommand<PlaceOrderResponse>;





