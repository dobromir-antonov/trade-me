using SharedKernel.Domain;

namespace Modules.Orders.Domain.ValueObjects;

public sealed class OrderId : ValueObject
{
    private OrderId(Guid value) => Value = value;

    public Guid Value { get; }

    public static OrderId Create(Guid value) => new(value);
    public static OrderId CreateNew() => new(Guid.NewGuid());

    public static implicit operator Guid(OrderId id) => id.Value;
    public static implicit operator OrderId(Guid id) => new(id);
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
