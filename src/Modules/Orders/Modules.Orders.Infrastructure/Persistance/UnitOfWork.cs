using Modules.Orders.Domain;

namespace Modules.Orders.Infrastructure.Persistance;

internal sealed class UnitOfWork(OrdersDbContext dbContext) : IUnitOfWork
{
    public int SaveChanges()
    {
        return dbContext.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

}