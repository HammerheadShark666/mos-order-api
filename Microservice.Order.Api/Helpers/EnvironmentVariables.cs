// Ignore Spelling: Jwt GRPC

namespace Microservice.Order.Api.Helpers;

public class EnvironmentVariables
{
    public static string JwtIssuer = Environment.GetEnvironmentVariable(Constants.JwtIssuer) ?? "Environment Variable JwtIssuer not found.";
    public static string JwtAudience = Environment.GetEnvironmentVariable(Constants.JwtAudience) ?? "Environment Variable JwtAudience not found.";
    public static string JwtSymmetricSecurityKey = Environment.GetEnvironmentVariable(Constants.JwtSymmetricSecurityKey) ?? "Environment Variable JwtSymmetricSecurityKey not found.";
    public static string GRPC_Book_Url = Environment.GetEnvironmentVariable(Constants.GRPC_Book_Url) ?? "Environment Variable GRPC_Book_Url not found.";
    public static string GRPC_CustomerAddress_Url = Environment.GetEnvironmentVariable(Constants.GRPC_CustomerAddress_Url) ?? "Environment Variable GRPC_CustomerAddress_Url not found.";

    public static string AzureServiceBusConnection => Environment.GetEnvironmentVariable(Constants.AzureServiceBusConnection) ?? "Environment Variable AzureServiceBusConnection not found.";
    public static string AzureServiceBusQueueOrderCompleted => Environment.GetEnvironmentVariable(Constants.AzureServiceBusQueueOrderCompleted) ?? "Environment Variable AzureServiceBusQueueOrderCompleted not found.";
}