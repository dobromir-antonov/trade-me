using Microsoft.EntityFrameworkCore;
using Modules.Price.Domain;
using System.Reflection;

namespace Modules.Price.Infrastructure.Persistance;

public sealed class PriceDbContext(DbContextOptions<PriceDbContext> options) : DbContext(options)
{
    public DbSet<Ticker> Tickers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

