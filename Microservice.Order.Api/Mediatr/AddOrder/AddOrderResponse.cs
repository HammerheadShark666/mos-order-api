namespace Microservice.Order.Api.MediatR.AddOrder;

public record AddOrderResponse(AddOrderOrderResponse order, List<AddOrderInvalidOrderItemResponse> invalidOrderItems);

public record AddOrderInvalidOrderItemResponse(Guid ProductId, int Quantity);

public record AddOrderOrderResponse(Guid Id, string OrderNumber, string AddressSurname, string AddressForename, List<AddOrderOrderItemResponse> OrderItems,
                                    decimal Total, string Status, string OrderDate, AddOrderAddressResponse OrderAddress);

public record AddOrderOrderItemResponse(string Name, int Quantity, decimal UnitPrice, Helpers.Enums.ProductType ProductType);

public record AddOrderAddressResponse(string AddressLine1, string AddressLine2, string AddressLine3, string TownCity,
                                        string County, string Postcode, string Country);