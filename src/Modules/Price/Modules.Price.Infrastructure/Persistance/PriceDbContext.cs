using Modules.Price.Domain;
using System.Collections.ObjectModel;

namespace Modules.Price.Infrastructure.Persistance;

public sealed class PriceDbContext
{
    public ReadOnlyCollection<Ticker> Tickers => _tickers.AsReadOnly();

    private static List<Ticker> _tickers = [
        Ticker.Create(
            id: TickerId.Create(Guid.Parse("351093cf-837a-422c-8202-b001587b04e6")),
            code:"APPL",
            name: "Apple"),

        Ticker.Create(
            id: TickerId.Create(Guid.Parse("65fb92d7-a296-4197-99d7-6b747827909d")), 
            code: "TSLA", 
            name: "Tesla"),

        Ticker.Create(
            id: TickerId.Create(Guid.Parse("99758f07-12b8-411c-b646-9162a54692dc")), 
            code: "NVDA", 
            name: "Nvidia")
    ];

}

