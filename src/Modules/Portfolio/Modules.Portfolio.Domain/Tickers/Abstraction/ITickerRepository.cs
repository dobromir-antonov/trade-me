using Modules.Portfolio.Domain.Tickers.ValueObjects;

namespace Modules.Portfolio.Domain.Tickers.Abstraction;

public interface ITickerRepository
{
    Task<Ticker?> GetByIdAsync(TickerId id, CancellationToken ct);
    Task AddAsync(Ticker ticker, CancellationToken ct);
    void Update(Ticker ticker);
}

