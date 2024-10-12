using Modules.Portfolio.Domain.UserPortfolios.ValueObjects;

namespace Modules.Portfolio.Domain.UserPortfolios.Abstraction;

public interface IUserPortfolioRepository
{
    Task<UserPortfolio?> GetByIdAsync(UserPortfolioId portfolioId, CancellationToken cancellationToken = default);
    Task AddAsync(UserPortfolio portfolio, CancellationToken cancellationToken = default);
    void Update(UserPortfolio portfolio);
}
