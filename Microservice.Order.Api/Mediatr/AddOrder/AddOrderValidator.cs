using FluentValidation;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Helpers;

namespace Microservice.Order.Api.MediatR.AddOrder;

public class AddOrderValidator : AbstractValidator<AddOrderRequest>
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;


        //customer has id

        //customer address has id

        //order items > 0



        //order items

           //quantity >0
           //Unit price
           //name


        //RuleFor(addOrderRequest => addOrderRequest).Must((addOrderRequest, cancellation) =>
        //{
        //    return OrderHelper.ValidISBN13(addOrderRequest.Isbn); 
        //}).WithMessage("Invalid ISBN code");

        //RuleFor(addOrderRequest => addOrderRequest).MustAsync(async (addOrderRequest, cancellation) =>
        //{
        //    return await IsbnExists(addOrderRequest.Isbn);
        //}).WithMessage("A order with this isbn already exists");

        //RuleFor(addOrderRequest => addOrderRequest.Title)
        //        .NotEmpty().WithMessage("Title is required.")
        //        .Length(1, 150).WithMessage("Title length between 1 and 150.");

        //RuleFor(addOrderRequest => addOrderRequest.Summary)
        //        .NotEmpty().WithMessage("Summary is required.")
        //        .Length(1, 2000).WithMessage("Summary length between 1 and 2000."); 
    }

    //protected async Task<bool> IsbnExists(string isbn)
    //{
    //    return !await _orderRepository.IsbnExistsAsync(isbn);
    //}
}