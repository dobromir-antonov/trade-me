using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Application.Portfolio.GetPortfolio;

namespace Modules.Portfolio.Application.Abstraction;

public interface IPortfolioReadOnlyDbContext
{
    DbSet<GetPortfolioResponse> UserPortfolios { get; }
}
