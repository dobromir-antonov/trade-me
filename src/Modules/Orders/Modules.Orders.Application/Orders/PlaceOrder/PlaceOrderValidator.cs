using FluentValidation;

namespace Modules.Orders.Application.Orders.PlaceOrder;

public sealed class PlaceOrderValidator : AbstractValidator<PlaceOrder>
{
    public PlaceOrderValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User is required");

        RuleFor(x => x.TickerId)
            .NotEmpty()
            .WithMessage("Ticker is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be positive number");
    }
}





