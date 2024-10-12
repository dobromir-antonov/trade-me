using SharedKernel.Domain;

namespace Modules.Price.Domain.Tickers;

public sealed class Ticker : AggregateRoot<TickerId>
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public decimal LastPrice { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    //EF Core
    private Ticker() { }

    private Ticker(TickerId id, string code, string name, decimal lastPrice, DateTime updatedOn) : base(id)
    {
        Code = code;
        Name = name;
        LastPrice = lastPrice;
        UpdatedOn = updatedOn;
    }

    public static Ticker Create(string code, string name, decimal lastPrice, DateTime updatedOn)
    {
        return new Ticker(TickerId.CreateNew(), code, name, lastPrice, updatedOn);
    }
}
