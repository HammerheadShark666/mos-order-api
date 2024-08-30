using Microservice.Order.Api.Helpers.Interfaces;

namespace Microservice.Order.Api.Helpers;

public class OrderHelper : IOrderHelper
{
    public string PaddedOrderNumber(int orderNumber)
    {
        return orderNumber.ToString().PadLeft(10, '0');
    }
}
