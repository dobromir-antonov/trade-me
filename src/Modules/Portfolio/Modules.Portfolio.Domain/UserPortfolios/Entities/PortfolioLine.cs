using Modules.Portfolio.Domain.UserPortfolios.ValueObjects;
using SharedKernel.Domain;

namespace Modules.Portfolio.Domain.UserPortfolios.Entities;

public sealed class PortfolioLine : Entity<PortfolioLineId>
{
    public Guid TickerId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public OrderSide Side { get; private set; }
    public Guid UserId { get; private set; }

    //EF Core
    private PortfolioLine() { }

    private PortfolioLine(PortfolioLineId id, Guid tickerId, int quantity, decimal price, OrderSide side, Guid userId) : base(id)
    {
        TickerId = tickerId;
        Quantity = quantity;
        Price = price;
        Side = side;
        UserId = userId;
    }

    public static PortfolioLine Create(Guid tickerId, int quantity, decimal price, OrderSide side, Guid userId)
    {
        return new PortfolioLine(PortfolioLineId.CreateNew(), tickerId, quantity, price, side, userId);
    }
}
