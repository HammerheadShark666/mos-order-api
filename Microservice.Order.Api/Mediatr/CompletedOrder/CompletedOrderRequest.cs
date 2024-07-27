using MediatR;
using Microservice.Order.Api.MediatR.AddOrder;

namespace Microservice.Order.Api.MediatR.CompletedOrder;

public record CompletedOrderRequest(Guid OrderId) : IRequest<CompletedOrderResponse>;