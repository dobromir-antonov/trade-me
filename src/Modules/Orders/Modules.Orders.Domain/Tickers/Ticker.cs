using SharedKernel.Domain;

namespace Modules.Orders.Domain.Tickers;

public sealed class Ticker : AggregateRoot<TickerId>
{
    public string Code { get; private set; }
    public decimal LastPrice { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    //EF Core
    private Ticker() { }

    private Ticker(TickerId id, string code, decimal lastPrice, DateTime updatedOn) : base(id)
    {
        Code = code;
        LastPrice = lastPrice;
        UpdatedOn = updatedOn;
    }

    public static Ticker Create(TickerId id, string code, decimal lastPrice, DateTime updatedOn)
    {
        var t = new Ticker(id, code, lastPrice, updatedOn);
        t.AddDomainEvent(new TickerCreatedEvent(t.Id));

        return t;
    }

    public void UpdatePrice(decimal price)
    {
        LastPrice = price;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new TickerPriceAdjustedEvent(Id, LastPrice));
    }
}
