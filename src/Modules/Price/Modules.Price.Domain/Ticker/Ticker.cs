using SharedKernel.Domain;

namespace Modules.Price.Domain;

public sealed class Ticker : AggregateRoot<TickerId>
{
    public string Name { get; private set; }

    //EF Core
    private Ticker() { }

    private Ticker(TickerId id, string name) : base(id)
    {
        Name = name;
    }

    public static Ticker Create(string name, string contactPerson, string email, string phoneNumber)
    {
        return new Ticker(TickerId.CreateNew(), name);
    }
}
