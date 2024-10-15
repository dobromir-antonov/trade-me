using SharedKernel.Messaging;

namespace Modules.Orders.IntegrationEvents
{
    public record OrderPlacedIntegrationEvent(Guid OrderId, Guid TickerId, int Quantity, decimal Price, int Side, Guid UserId) : IIntegrationEvent;
}
