using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Domain.Tickers;
using Modules.Portfolio.Domain.Tickers.Abstraction;
using Modules.Portfolio.Domain.Tickers.ValueObjects;

namespace Modules.Portfolio.Infrastructure.Persistance.Repositories;

public class TickerRepository(PortfolioDbContext dbContext) : ITickerRepository
{
    public async Task AddAsync(Ticker ticker, CancellationToken ct)
    {
        await dbContext.AddAsync(ticker, ct);
    }

    public Task<Ticker?> GetByIdAsync(TickerId id, CancellationToken ct)
    {
        return dbContext.Tickers.FirstOrDefaultAsync(x => x.Id == id.Value, ct);
    }

    public Task<decimal> GetPriceByIdAsync(TickerId id, CancellationToken ct)
    {
        return dbContext.Tickers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.LastPrice)
            .FirstOrDefaultAsync(ct);
    }

    public void Update(Ticker ticker)
    {
        dbContext.Update(ticker);
    }
}
