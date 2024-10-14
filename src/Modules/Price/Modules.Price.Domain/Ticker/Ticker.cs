using SharedKernel.Domain;

namespace Modules.Price.Domain;

public sealed class Ticker : AggregateRoot<TickerId>
{
    public string Code { get; private set; }
    public string Name { get; private set; }

    //EF Core
    private Ticker() { }

    private Ticker(TickerId id, string code, string name) : base(id)
    {
        Code = code;
        Name = name;
    }

    public static Ticker Create(string code, string name)
    {
        return new Ticker(TickerId.CreateNew(), code, name);
    }

    [Obsolete("for seed purpose only")]
    public static Ticker Create(TickerId id, string code, string name)
    {
        return new Ticker(id, code, name);
    }
}
