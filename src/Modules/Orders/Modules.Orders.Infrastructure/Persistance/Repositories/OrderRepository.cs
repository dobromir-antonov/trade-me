using Modules.Orders.Domain;

namespace Modules.Orders.Infrastructure.Persistance.Repositories;

public class OrderRepository(OrdersDbContext context) : IOrderRepository
{
    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(order, cancellationToken);
    }
}
