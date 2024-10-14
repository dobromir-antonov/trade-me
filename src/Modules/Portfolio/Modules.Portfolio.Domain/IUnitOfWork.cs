namespace Modules.Portfolio.Domain;

public interface IUnitOfWork
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
