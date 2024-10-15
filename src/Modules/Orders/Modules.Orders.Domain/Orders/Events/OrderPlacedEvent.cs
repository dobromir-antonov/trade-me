using SharedKernel.Domain;

namespace Modules.Orders.Domain.Orders;

public record OrderPlacedEvent(Guid OrderId, Guid TickerId, int Quantity, decimal Price, Guid UserId) : IDomainEvent;
