namespace Modules.Portfolio.Domain.Orders.Abstraction;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
}

