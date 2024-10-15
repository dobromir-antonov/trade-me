using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Application.Abstraction;
using Modules.Portfolio.Application.Portfolio.GetPortfolio;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure.Persistance;

public sealed class PortfolioReadOnlyDbContext : DbContext, IPortfolioReadOnlyDbContext
{
    public DbSet<GetPortfolioResponse> UserPortfolios { get; set; }

    public PortfolioReadOnlyDbContext(DbContextOptions<PortfolioReadOnlyDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<GetPortfolioResponse>().HasNoKey();

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
        => throw new InvalidOperationException("This context is read-only");

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("This context is read-only");
}



