using SharedKernel.Domain;

namespace Modules.Orders.Domain.Orders;

public record OrderPlacedEvent(Guid OrderId, Guid TickerId, int Quantity, decimal Price, int Side, Guid UserId) : IDomainEvent;
