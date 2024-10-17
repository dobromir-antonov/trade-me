using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Modules.Orders.Application;
using Modules.Orders.Domain;
using Modules.Orders.Domain.Tickers;
using Modules.Orders.Infrastructure.Outbox;
using SharedKernel.Domain;
using System.Reflection;

namespace Modules.Orders.Infrastructure.Persistance;

public class OrdersDbContext(IPublisher publisher, DbContextOptions<OrdersDbContext> options) : DbContext(options), IUnitOfWork
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IDomainEvent[] domainEvents = ChangeTracker
           .Entries<IHasDomainEvents>()
           .SelectMany(e => e.Entity.PopDomainEvents())
           .ToArray();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

public class OrdersDbContextFactory : IDesignTimeDbContextFactory<OrdersDbContext>
{
    public OrdersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdersDbContext>();
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=trademe;Username=postgres;Password=postgrespw");

        return new OrdersDbContext(null, optionsBuilder.Options);
    }
}