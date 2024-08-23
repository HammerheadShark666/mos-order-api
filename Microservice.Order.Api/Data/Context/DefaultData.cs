using Microservice.Order.Api.Domain;

namespace Microservice.Order.Api.Data.Context;

public class DefaultData
{
    public static List<Domain.OrderStatus> GetOrderStatusDefaultData()
    {
        return new List<Domain.OrderStatus>()
        {
            new() { Id = Helpers.Enums.OrderStatus.Created, Status = "Created" },
            new() { Id = Helpers.Enums.OrderStatus.Paid, Status = "Paid" },
            new() { Id = Helpers.Enums.OrderStatus.Dispatched, Status = "Dispatched" },
            new() { Id = Helpers.Enums.OrderStatus.Recieved, Status = "Recieved" },
            new() { Id = Helpers.Enums.OrderStatus.Completed, Status = "Completed" }
        };
    }

    public static List<Domain.ProductType> GetProductTypeDefaultData()
    {
        return new List<Domain.ProductType>()
        {
            new() { Id = Api.Helpers.Enums.ProductType.Book, Name = "Book" },
            new() { Id = Api.Helpers.Enums.ProductType.Music, Name = "Music" }
        };
    }

    public static List<Domain.Order> GetOrderDefaultData()
    {
        return new List<Domain.Order>()
        {
            CreateOrder(new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), "Intergration_Test", "Intergration_Test", 1, new List<OrderItem>(), Helpers.Enums.OrderStatus.Created, 25.99m),
            CreateOrder(new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), "Intergration_Test", "Intergration_Test", 2, new List<OrderItem>(), Helpers.Enums.OrderStatus.Paid, 19.98m),
            CreateOrder(new Guid("6f3e950c-5502-491f-911d-02e112318705"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), "Intergration_Test", "Intergration_Test", 3, new List<OrderItem>(), Helpers.Enums.OrderStatus.Dispatched, 2.50m),
            CreateOrder(new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new Guid("929eaf82-e4fd-4efe-9cae-ce4d7e32d159"), new Guid("b88ef4ce-739f-4c1b-b6d6-9d0727515de8"), "Intergration_Test2", "Intergration_Test2", 4, new List<OrderItem>(), Helpers.Enums.OrderStatus.Recieved, 51.47m),
            CreateOrder(new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("929eaf82-e4fd-4efe-9cae-ce4d7e32d159"), new Guid("b88ef4ce-739f-4c1b-b6d6-9d0727515de8"), "Intergration_Test2", "Intergration_Test2", 5, new List<OrderItem>(), Helpers.Enums.OrderStatus.Created, 24.98m),

         };
    }

    public static List<OrderItem> GetOrderItemDefaultData()
    {
        return new List<OrderItem>()
        {
            CreateOrderItem(new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), "Infinity Kings", Helpers.Enums.ProductType.Book, 1, 9.99m),
            CreateOrderItem(new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", Helpers.Enums.ProductType.Book, 1, 7.50m),
            CreateOrderItem(new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("6131ce7e-fb11-4608-a3d3-f01caee2c465"), "Infinity Reaper", Helpers.Enums.ProductType.Book, 1, 8.50m),
            CreateOrderItem(new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), new Guid("37544155-da95-49e8-b7fe-3c937eb1de98"), "Wild Love", Helpers.Enums.ProductType.Book, 2, 9.99m),
            CreateOrderItem(new Guid("6f3e950c-5502-491f-911d-02e112318705"), new Guid("f3fcab1f-1c11-47f5-9e11-7868a88408e6"), "Thunderhead", Helpers.Enums.ProductType.Book, 1, 2.50m),
            CreateOrderItem(new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new Guid("23608dce-2142-4d2b-b909-948316b5efaf"), "Scythe", Helpers.Enums.ProductType.Book, 1, 3.50m),
            CreateOrderItem(new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new Guid("ecf65c56-5670-473b-9f20-fb0b191c2f0f"), "Saltblood", Helpers.Enums.ProductType.Book, 3, 15.99m),
            CreateOrderItem(new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), "Infinity Kings", Helpers.Enums.ProductType.Book, 2, 9.99m),
            CreateOrderItem(new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", Helpers.Enums.ProductType.Book, 2, 7.50m)
        };
    }

    private static Domain.Order CreateOrder(Guid id, Guid customerId, Guid customerAddressId, string AddressSurname, string AddressForename, int orderNumber, List<OrderItem> orderItems, Helpers.Enums.OrderStatus orderStatusId, decimal total)
    {
        return new Domain.Order
        {
            Id = id,
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressSurname = AddressSurname,
            AddressForename = AddressForename,
            OrderNumber = orderNumber,
            OrderItems = orderItems,
            OrderStatusId = orderStatusId,
            Total = total

        };
    }

    private static OrderItem CreateOrderItem(Guid orderId, Guid productId, string name, Helpers.Enums.ProductType productType, int quantity, decimal unitPrice)
    {
        return new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Name = name,
            ProductTypeId = productType,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}