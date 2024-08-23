namespace Microservice.Order.Api.MediatR.GetOrder;

public record GetOrderResponse(Guid Id, string OrderNumber, string AddressSurname, string AddressForename, List<GetOrderOrderItemResponse> OrderItems,
                                    decimal Total, string Status, string OrderDate, GetOrderAddressResponse OrderAddress);

public record GetOrderOrderItemResponse(string Name, int Quantity, decimal UnitPrice, string Type);

public record GetOrderAddressResponse(string AddressLine1, string AddressLine2, string AddressLine3, string TownCity,
                                        string County, string Postcode, string Country);