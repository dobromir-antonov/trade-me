using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Application.Abstraction;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure.Persistance;

public sealed class PortfolioReadOnlyDbContext : DbContext, IPortfolioReadOnlyDbContext
{
    public PortfolioReadOnlyDbContext(DbContextOptions<PortfolioReadOnlyDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema(Schema.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public IQueryable<T> SqlQuery<T>(FormattableString query) => Database.SqlQuery<T>(query);

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
        => throw new InvalidOperationException("This context is read-only");

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("This context is read-only");
}



