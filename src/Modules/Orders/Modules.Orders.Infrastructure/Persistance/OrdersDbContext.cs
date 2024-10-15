using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Modules.Orders.Domain;
using Modules.Orders.Domain.Tickers;
using Modules.Orders.Infrastructure.Outbox;
using System.Reflection;

namespace Modules.Orders.Infrastructure.Persistance;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Ticker> Tickers { get; set; }
    public DbSet<OutboxIntegrationEvent> OutboxIntegrationEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}

public class OrdersDbContextFactory : IDesignTimeDbContextFactory<OrdersDbContext>
{
    public OrdersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdersDbContext>();
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=trademe;Username=postgres;Password=postgrespw");

        return new OrdersDbContext(optionsBuilder.Options);
    }
}