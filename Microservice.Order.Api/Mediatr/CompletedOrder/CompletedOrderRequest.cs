using MediatR;

namespace Microservice.Order.Api.MediatR.CompletedOrder;

public record CompletedOrderRequest(Guid OrderId) : IRequest<CompletedOrderResponse>;