using FluentValidation;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.MediatR.CompletedOrder;

namespace Microservice.Customer.Api.MediatR.AddCustomer;

public class completedOrderValidator : AbstractValidator<CompletedOrderRequest>
{
    private readonly IOrderRepository _orderRepository;

    public completedOrderValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(completedOrderRequest => completedOrderRequest).MustAsync(async (completedOrderRequest, cancellation) => {
            return await OrderNotFound(completedOrderRequest);
        }).WithMessage("Order not found, may have already been completed.");

        RuleFor(completedOrderRequest => completedOrderRequest).MustAsync(async (completedOrderRequest, cancellation) => {
            return await OrderAlreadyClosed(completedOrderRequest);
        }).WithMessage("Order already closed.");       
    }

    protected async Task<bool> OrderAlreadyClosed(CompletedOrderRequest completedOrderRequest)
    {
        return !await _orderRepository.OrderIsClosedAsync(completedOrderRequest.OrderId);
    }

    protected async Task<bool> OrderNotFound(CompletedOrderRequest completedOrderRequest)
    {
        return !await _orderRepository.OrderNotFound(completedOrderRequest.OrderId);
    }
}