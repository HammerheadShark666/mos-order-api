using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interceptors;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.Protos;

namespace Microservice.Order.Api.Grpc;

public class CustomerAddressService(IJwtHelper jwtHelper,
                                    ICustomerHttpAccessor customerHttpAccessor) : ICustomerAddressService
{
    private IJwtHelper _jwtHelper { get; set; } = jwtHelper;
    private ICustomerHttpAccessor _customerHttpAccessor { get; set; } = customerHttpAccessor;

    public async Task<CustomerAddressResponse> GetCustomerAddressAsync(Guid addressId)
    {
        ILoggerFactory _loggerFactory = LoggerFactory.Create(b => b.AddConsole());
        ILogger _logger = _loggerFactory.CreateLogger<CustomerAddressService>();

        using var channel = GrpcChannel.ForAddress(EnvironmentVariables.GRPC_CustomerAddress_Url, new GrpcChannelOptions 
        { 
            HttpHandler = GetClientHandler(),
            LoggerFactory = _loggerFactory
        });

        var invoker = channel.Intercept(new ClientLoggingInterceptor(_loggerFactory));  
        var client = new CustomerAddressGrpc.CustomerAddressGrpcClient(invoker);
        var response = await client.GetCustomerAddressAsync(GetCustomerAddressRequest(GetCustomerId(), addressId), GetGrpcHeaders());

        _logger.LogInformation("Customer address found: {customerId} / {addressId}", GetCustomerId(), addressId);

        return response;
    }

    private Guid GetCustomerId()
    {
        var customerId = _customerHttpAccessor.CustomerId;
        if (Guid.Empty.Equals(customerId))
        {
            throw new NotFoundException("Customer not found.");
        }

        return customerId;
    }

    private CustomerAddressRequest GetCustomerAddressRequest(Guid customerId, Guid addressId)
    {
        return new CustomerAddressRequest()
        {
            CustomerId = customerId.ToString(),
            AddressId = addressId.ToString()
        };
    }

    private Metadata GetGrpcHeaders()
    {
        var headers = new Metadata();
        headers.Add("Authorization", $"Bearer {_jwtHelper.GenerateJwtToken()}");

        return headers;
    }

    private HttpClientHandler GetClientHandler()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; 

        return handler;
    }
}