using FluentValidation;

namespace Microservice.Order.Api.MediatR.AddOrder;

public class AddOrderValidator : AbstractValidator<AddOrderRequest>
{
    public AddOrderValidator()
    {
        RuleFor(addOrderRequest => addOrderRequest.OrderItems.Count)
                .NotEqual(0)
                .WithMessage("Order has no order items.");
    }
}