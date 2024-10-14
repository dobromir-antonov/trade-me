using FluentResults;
using Modules.Orders.Domain.Orders;
using Modules.Orders.Domain.ValueObjects;
using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public class Order : AggregateRoot<OrderId>
{
    public TickerId TickerId { get; private set; }  
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public Guid UserId { get; private set; }

    //EF Core
    private Order() { }

    private Order(OrderId id, TickerId tickerId, int quantity, decimal price, Guid userId) : base(id)
    {
        TickerId = tickerId;
        Quantity = quantity;
        Price = price;
        UserId = userId;
    }

    public static Result<Order> Create(TickerId tickerId, int quantity, decimal price, Guid userId)
    {
        var order = new Order(OrderId.CreateNew(), tickerId, quantity, price, userId);

        if (order.Price <= 0)
        { 
            return OrderErrors.InvalidPrice;
        }

        if (order.Quantity <= 0)
        {
            return OrderErrors.InvalidQuantity;
        }

        order.AddDomainEvent(new OrderPlacedDomainEvent(order.Id));

        return order;
    }
    
}

