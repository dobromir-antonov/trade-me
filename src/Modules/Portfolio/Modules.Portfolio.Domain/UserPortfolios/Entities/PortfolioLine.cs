using Modules.Portfolio.Domain.UserPortfolios.ValueObjects;
using SharedKernel.Domain;

namespace Modules.Portfolio.Domain.UserPortfolios.Entities;

public sealed class PortfolioLine : Entity<PortfolioLineId>
{
    public Guid TickerId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    //EF Core
    private PortfolioLine() { }

    private PortfolioLine(PortfolioLineId id, Guid tickerId, int quantity, decimal price) : base(id)
    {
        TickerId = tickerId;
        Quantity = quantity;
        Price = price;
    }

    public static PortfolioLine Create(Guid tickerId, int quantity, decimal price)
    {
        return new PortfolioLine(PortfolioLineId.CreateNew(), tickerId, quantity, price);
    }
}
