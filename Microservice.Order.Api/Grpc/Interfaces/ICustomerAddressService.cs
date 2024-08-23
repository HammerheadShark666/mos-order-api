using Microservice.Order.Api.Protos;

namespace Microservice.Order.Api.Grpc.Interfaces;

public interface ICustomerAddressService
{
    Task<CustomerAddressResponse> GetCustomerAddressAsync(Guid id);
}