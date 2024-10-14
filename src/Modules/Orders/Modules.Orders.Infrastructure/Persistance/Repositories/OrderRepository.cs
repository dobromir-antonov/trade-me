using Modules.Orders.Domain;

namespace Modules.Orders.Infrastructure.Persistance.Repositories;

public class OrderRepository(OrdersDbContext dbContext) : IOrderRepository
{
    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(order, cancellationToken);
    }
}
