namespace Microservice.Order.Api.Helpers.Interfaces;

public interface IAzureServiceBusHelper
{
    Task SendMessage(string queue, string data);
}