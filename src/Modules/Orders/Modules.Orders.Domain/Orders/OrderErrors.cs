using FluentResults;

namespace Modules.Orders.Domain.Orders;

internal static class OrderErrors
{
    public static readonly Result InvalidPrice = new Error("Price has to be positive number");
    public static readonly Result InvalidQuantity = new Error("Quantity has to be positive number");
}
