using Modules.Orders.Domain.Tickers;
using Modules.Orders.Domain.Tickers.ValueObjects;
using System.Threading;

namespace Modules.Orders.Domain.Tickers.Abstraction;

public interface ITickerRepository
{
    Task<Ticker?> GetByIdAsync(TickerId id, CancellationToken ct);
    Task<decimal> GetPriceByIdAsync(TickerId id, CancellationToken ct);

    Task AddAsync(Ticker ticker, CancellationToken ct);
    void Update(Ticker ticker);
}

