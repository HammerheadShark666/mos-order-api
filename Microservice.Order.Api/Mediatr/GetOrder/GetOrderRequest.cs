using MediatR;

namespace Microservice.Order.Api.MediatR.GetOrder;

public record GetOrderRequest(Guid Id) : IRequest<GetOrderResponse>;