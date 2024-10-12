namespace Modules.Orders.Domain;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
}

