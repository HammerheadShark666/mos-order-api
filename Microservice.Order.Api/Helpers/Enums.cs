namespace Microservice.Order.Api.Helpers;

public class Enums
{
    public enum Role
    {
        SuperAdmin,
        Admin,
        Moderator,
        User
    }

    public enum ProductType
    {
        NotFound = 0,
        Book = 1,
        Music = 2
    }

    public enum OrderStatus
    {
        Created = 1,
        Paid = 2,
        Dispatched = 3,
        Recieved = 4,
        Completed = 5
    }

    public enum ErrorType
    {
        Error,
        Information
    }
}