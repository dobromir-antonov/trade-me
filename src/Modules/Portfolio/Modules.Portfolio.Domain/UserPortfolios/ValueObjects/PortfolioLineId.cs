using SharedKernel.Domain;

namespace Modules.Portfolio.Domain.UserPortfolios.ValueObjects;

public sealed class PortfolioLineId : ValueObject
{
    private PortfolioLineId(Guid value) => Value = value;

    public Guid Value { get; }

    public static PortfolioLineId Create(Guid value) => new(value);
    public static PortfolioLineId CreateNew() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator Guid(PortfolioLineId id) => id.Value;
    public static implicit operator PortfolioLineId(Guid id) => new(id);
}
