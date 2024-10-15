using SharedKernel.Domain;

namespace Modules.Portfolio.Domain.Tickers.ValueObjects;

public sealed class TickerId : ValueObject
{
    private TickerId(Guid value) => Value = value;

    public Guid Value { get; }

    public static TickerId Create(Guid value) => new(value);
    public static TickerId CreateNew() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator Guid(TickerId id) => id.Value;
    public static implicit operator TickerId(Guid id) => new(id);
}
