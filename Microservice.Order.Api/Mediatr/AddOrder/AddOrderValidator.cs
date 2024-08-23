using FluentValidation;
using Microservice.Order.Api.Data.Repository.Interfaces;

namespace Microservice.Order.Api.MediatR.AddOrder;

public class AddOrderValidator : AbstractValidator<AddOrderRequest>
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(addOrderRequest => addOrderRequest.OrderItems.Count)
                .NotEqual(0)
                .WithMessage("Order has no order items.");
    }
}