using FluentResults;
using Modules.Portfolio.Domain.Orders.ValueObjects;
using Modules.Portfolio.Domain.Tickers.ValueObjects;
using SharedKernel.Domain;

namespace Modules.Portfolio.Domain.Orders;

public class Order : AggregateRoot<OrderId>
{
    public TickerId TickerId { get; private set; }  
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public OrderSide Side { get; private set; }
    public Guid UserId { get; private set; }

    //EF Core
    private Order() { }

    private Order(OrderId id, TickerId tickerId, int quantity, decimal price, OrderSide side, Guid userId) : base(id)
    {
        TickerId = tickerId;
        Quantity = quantity;
        Price = price;
        Side = side;
        UserId = userId;
    }

    public static Result<Order> Create(OrderId orderId, TickerId tickerId, int quantity, decimal price, OrderSide side, Guid userId)
    {
        var order = new Order(orderId, tickerId, quantity, price, side, userId);
        return order;
    }
    
}

