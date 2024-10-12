using SharedKernel.Domain;

namespace Modules.Portfolio.Domain.UserPortfolios.ValueObjects;

public sealed class UserPortfolioId : ValueObject
{
    private UserPortfolioId(Guid value) => Value = value;

    public Guid Value { get; }

    public static UserPortfolioId Create(Guid value) => new(value);
    public static UserPortfolioId CreateNew() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator Guid(UserPortfolioId id) => id.Value;
    public static implicit operator UserPortfolioId(Guid id) => new(id);
}
