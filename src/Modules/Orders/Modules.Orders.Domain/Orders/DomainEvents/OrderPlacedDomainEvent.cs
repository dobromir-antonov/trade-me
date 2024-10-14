using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public record OrderPlacedDomainEvent(Guid OrderId) : IDomainEvent;
