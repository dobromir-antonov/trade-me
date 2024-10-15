namespace Modules.Portfolio.Application.Abstraction;

public interface IPortfolioReadOnlyDbContext
{
    IQueryable<T> SqlQuery<T>(FormattableString query);
}
