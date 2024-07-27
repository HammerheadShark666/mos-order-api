namespace Microservice.Order.Api.Helpers;

public class Constants
{
    public const string JwtIssuer = "JWT_ISSUER";
    public const string JwtAudience = "JWT_AUDIENCE";
    public const string JwtSymmetricSecurityKey = "JWT_SYMMETRIC_SECURITY_KEY";

    public const string GRPC_Book_Url = "GRPC_BOOK_URL";
    public const string GRPC_CustomerAddress_Url = "GRPC_CUSTOMER_ADDRESS_URL";

    public const string AzureServiceBusConnection = "AZURE_SERVICE_BUS_CONNECTION";
    public const string AzureServiceBusQueueOrderCompleted = "order-completed";

    public const string DatabaseConnectionString = "SQLAZURECONNSTR_ORDER";

    public const string DateFormat_ddMMyyyy = "dd/MM/yyyy";
}