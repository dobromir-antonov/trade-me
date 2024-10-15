using Modules.Orders.Domain;
using Modules.Orders.Domain.Orders.Abstraction;

namespace Modules.Orders.Infrastructure.Persistance.Repositories;

public class OrderRepository(OrdersDbContext dbContext) : IOrderRepository
{
    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(order, cancellationToken);
    }
}
