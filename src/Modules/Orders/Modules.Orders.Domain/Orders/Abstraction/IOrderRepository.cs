namespace Modules.Orders.Domain.Orders.Abstraction;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
}

