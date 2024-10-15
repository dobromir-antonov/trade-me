using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Domain.UserPortfolios;
using Modules.Portfolio.Domain.UserPortfolios.Abstraction;
using Modules.Portfolio.Domain.UserPortfolios.ValueObjects;

namespace Modules.Portfolio.Infrastructure.Persistance.Repositories;

//public sealed class UserPortfolioRepository(PortfolioDbContext dbContext) : IUserPortfolioRepository
//{
//    public Task<UserPortfolio?> GetByIdAsync(UserPortfolioId userPortfolioId, CancellationToken cancellationToken = default)
//    { 
//        return dbContext.Portfolios
//            .FirstOrDefaultAsync(x => x.Id == userPortfolioId, cancellationToken);
//    }

//    public async Task AddAsync(UserPortfolio portfolio, CancellationToken cancellationToken = default)
//    {
//        await dbContext.Portfolios.AddAsync(portfolio, cancellationToken);
//    }

//    public void Update(UserPortfolio portfolio)
//    {
//        dbContext.Portfolios.Update(portfolio);
//    }
//}

