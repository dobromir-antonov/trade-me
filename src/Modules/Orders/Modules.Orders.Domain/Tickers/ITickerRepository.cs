using Modules.Orders.Domain.Tickers;
using System.Threading;

namespace Modules.Orders.Domain;

public interface ITickerRepository
{
    Task<Ticker?> GetByIdAsync(TickerId id, CancellationToken ct);
    Task<decimal> GetPriceByIdAsync(TickerId id, CancellationToken ct);

    Task AddAsync(Ticker ticker, CancellationToken ct);
    void Update(Ticker ticker);
}

