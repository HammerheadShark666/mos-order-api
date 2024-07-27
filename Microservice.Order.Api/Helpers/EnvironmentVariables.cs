namespace Microservice.Order.Api.Helpers;

public class EnvironmentVariables
{
    public static string JwtIssuer = Environment.GetEnvironmentVariable(Constants.JwtIssuer);
    public static string JwtAudience = Environment.GetEnvironmentVariable(Constants.JwtAudience);
    public static string JwtSymmetricSecurityKey = Environment.GetEnvironmentVariable(Constants.JwtSymmetricSecurityKey);
    public static string GRPC_Book_Url = Environment.GetEnvironmentVariable(Constants.GRPC_Book_Url);
    public static string GRPC_CustomerAddress_Url = Environment.GetEnvironmentVariable(Constants.GRPC_CustomerAddress_Url);

    public static string AzureServiceBusConnection => Environment.GetEnvironmentVariable(Constants.AzureServiceBusConnection);
}