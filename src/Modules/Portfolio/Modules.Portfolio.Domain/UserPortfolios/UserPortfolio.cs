using Modules.Portfolio.Domain.UserPortfolios.Entities;
using Modules.Portfolio.Domain.UserPortfolios.ValueObjects;
using SharedKernel.Domain;
using System.Collections.ObjectModel;

namespace Modules.Portfolio.Domain.UserPortfolios;

public sealed class UserPortfolio : AggregateRoot<UserPortfolioId>
{
    public decimal Total { get; private set; }
    public ReadOnlyCollection<PortfolioLine> Lines { get; private set; }

    //EF Core
    private UserPortfolio() { }

    private UserPortfolio(UserPortfolioId id) : base(id)
    {
    }

    public static UserPortfolio Create(UserPortfolioId id)
    {
        return new UserPortfolio(id);
    }


    public void AddLine(PortfolioLine line)
    { 
    
    }
}
