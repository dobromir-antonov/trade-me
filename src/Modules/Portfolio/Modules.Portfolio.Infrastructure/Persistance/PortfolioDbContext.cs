using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Domain;
using Modules.Portfolio.Domain.UserPortfolios;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure.Persistance;

public sealed class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<UserPortfolio> Portfolios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

