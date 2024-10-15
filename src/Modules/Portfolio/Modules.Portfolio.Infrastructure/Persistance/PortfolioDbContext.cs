using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Modules.Portfolio.Domain;
using Modules.Portfolio.Domain.Orders;
using Modules.Portfolio.Domain.Tickers;
using Modules.Portfolio.Domain.UserPortfolios;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure.Persistance;

public sealed class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Ticker> Tickers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

public class PortfolioDbContextFactory : IDesignTimeDbContextFactory<PortfolioDbContext>
{
    public PortfolioDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PortfolioDbContext>();
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=trademe;Username=postgres;Password=postgrespw");

        return new PortfolioDbContext(optionsBuilder.Options);
    }
}

