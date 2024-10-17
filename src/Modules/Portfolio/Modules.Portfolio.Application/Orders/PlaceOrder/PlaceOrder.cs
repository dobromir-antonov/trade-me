using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Orders.PlaceOrder;

public sealed record PlaceOrder(Guid OrderId, Guid TickerId, int Quantity, decimal Price, int Side, Guid UserId) : ICommand;





