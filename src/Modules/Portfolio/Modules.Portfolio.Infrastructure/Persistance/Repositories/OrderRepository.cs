using Modules.Portfolio.Domain.Orders;
using Modules.Portfolio.Domain.Orders.Abstraction;

namespace Modules.Portfolio.Infrastructure.Persistance.Repositories;

public class OrderRepository(PortfolioDbContext dbContext) : IOrderRepository
{
    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(order, cancellationToken);
    }
}
