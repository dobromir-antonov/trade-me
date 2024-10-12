using Modules.Orders.Domain.ValueObjects;
using SharedKernel.Domain;

namespace Modules.Orders.Domain;

public class Order : AggregateRoot<OrderId>
{
    public string Ticker { get; private set; }  
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public Guid UserId { get; private set; }

    private Order(OrderId id, string ticker, int quantity, decimal price, Guid userId) : base(id)
    {
        Ticker = ticker;
        Quantity = quantity;
        Price = price;
        UserId = userId;
    }

    public static Order Create(string ticker, int quantity, decimal price, Guid userId)
    {
        var order = new Order(OrderId.CreateNew(), ticker, quantity, price, userId);

        order.AddDomainEvent(new OrderPlacedDomainEvent(order.Id));

        return order;
    }
    
}

