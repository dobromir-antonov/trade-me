using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain;
using SharedKernel.Application.Abstraction.Data;
using System.Reflection;

namespace Modules.Orders.Infrastructure.Persistance;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OutboxIntegrationEvent> OutboxIntegrationEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
