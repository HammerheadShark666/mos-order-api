using MediatR;
using static Microservice.Order.Api.Helpers.Enums;

namespace Microservice.Order.Api.MediatR.AddOrder;

public record AddOrderRequest(Guid CustomerId, Guid CustomerAddressId, string AddressSurname, string AddressForename, List<AddOrderItemRequest> OrderItems) : IRequest<AddOrderResponse>;

public record AddOrderItemRequest(Guid Id, int Quantity, ProductType ProductType);