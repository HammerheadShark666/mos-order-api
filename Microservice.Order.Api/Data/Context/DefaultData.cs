using Microservice.Order.Api.Domain;

namespace Microservice.Order.Api.Data.Context;

public class DefaultData
{
    public static List<Domain.OrderStatus> GetOrderStatusDefaultData()
    {
        return new List<Domain.OrderStatus>()
        {
            new Domain.OrderStatus { Id = Helpers.Enums.OrderStatus.Created, Status = "Created" },
            new Domain.OrderStatus { Id = Helpers.Enums.OrderStatus.Paid, Status = "Paid" },
            new Domain.OrderStatus { Id = Helpers.Enums.OrderStatus.Dispatched, Status = "Dispatched" },
            new Domain.OrderStatus { Id = Helpers.Enums.OrderStatus.Recieved, Status = "Recieved" },
            new Domain.OrderStatus { Id = Helpers.Enums.OrderStatus.Completed, Status = "Completed" }
        };
    }

    public static List<Domain.ProductType> GetProductTypeDefaultData()
    {
        return new List<Domain.ProductType>()
        {
            new Domain.ProductType { Id = Api.Helpers.Enums.ProductType.Book, Name = "Book" },
            new Domain.ProductType { Id = Api.Helpers.Enums.ProductType.Music, Name = "Music" }
        };
    }

    public static List<Domain.Order> GetOrderDefaultData()
    {
        return new List<Domain.Order>()
        {
            CreateOrder(new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("bb472ce8-edfd-4b90-8f11-40f3eeed778b"), new Guid("9a11e147-f416-4063-a5ed-94bae3bce423"), "West", "John", 1, new List<OrderItem>(), Helpers.Enums.OrderStatus.Created, 25.99m),
            CreateOrder(new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new Guid("eeed04b3-538b-4677-887b-77f0803958a6"), "Johnson", "Kelly", 2, new List<OrderItem>(), Helpers.Enums.OrderStatus.Paid, 19.98m),
            CreateOrder(new Guid("6f3e950c-5502-491f-911d-02e112318705"), new Guid("82c5ba18-d049-49a0-83aa-a2a9840b08ad"), new Guid("2bc333d2-8f8c-46bd-b454-b0735b662bea"), "Willow", "Michael", 3, new List<OrderItem>(), Helpers.Enums.OrderStatus.Dispatched, 2.50m),
            CreateOrder(new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new Guid("bb472ce8-edfd-4b90-8f11-40f3eeed778b"), new Guid("9a11e147-f416-4063-a5ed-94bae3bce423"), "Harper", "Lillie", 4, new List<OrderItem>(), Helpers.Enums.OrderStatus.Recieved, 51.47m),
            CreateOrder(new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("39c2080b-18ca-4974-8937-f9d758b89bac"), new Guid("97d31501-4008-4b1a-9aeb-71d4cea31059"), "Mortenson", "Burt", 5, new List<OrderItem>(), Helpers.Enums.OrderStatus.Created, 24.98m),
            CreateOrder(new Guid("34335ca4-ebcd-4a2e-b54a-af9feb09535d"), new Guid("453b920e-e8b0-4c5e-bd44-77c4cd75771d"), new Guid("14e016ec-3935-431f-88a3-17b55ad99198"), "Keltso", "Grace", 6, new List<OrderItem>(), Helpers.Enums.OrderStatus.Paid, 19.98m),
            CreateOrder(new Guid("54d54c4e-d774-4b01-af87-0d0b94510767"), new Guid("4de2c877-5cdb-4153-9e49-4a8f77d910e9"), new Guid("870062c2-09c1-4fd4-8066-7d149b5cc86c"), "Arbring", "Rachel", 7, new List<OrderItem>(), Helpers.Enums.OrderStatus.Completed, 28.78m),
            CreateOrder(new Guid("07a2ab81-7651-484f-945e-08074e7662bc"), new Guid("160429ab-7d0b-464e-8042-cec3218c014c"), new Guid("0a1cbffa-967f-4338-a2c1-e80238d61a16"), "Frown", "Ellen", 8, new List<OrderItem>(), Helpers.Enums.OrderStatus.Completed, 12.99m),
            CreateOrder(new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"), new Guid("3ea4739c-a8dc-4c59-b3ba-c6104024c24e"), new Guid("25ed9fe9-4c1d-414e-b6b3-bcf725dce00b"), "Gordonson", "William", 9, new List<OrderItem>(), Helpers.Enums.OrderStatus.Completed, 59.96m),
            CreateOrder(new Guid("69e3878e-8293-4e81-8791-31328e2a3907"), new Guid("82c5ba18-d049-49a0-83aa-a2a9840b08ad"), new Guid("2bc333d2-8f8c-46bd-b454-b0735b662bea"), "Yot", "Sano", 10, new List<OrderItem>(), Helpers.Enums.OrderStatus.Completed, 10.00m)
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
            CreateOrderItem(new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", Helpers.Enums.ProductType.Book, 2, 7.50m),
            CreateOrderItem(new Guid("34335ca4-ebcd-4a2e-b54a-af9feb09535d"), new Guid("37544155-da95-49e8-b7fe-3c937eb1de98"), "Wild Love", Helpers.Enums.ProductType.Book, 2, 9.99m),
            CreateOrderItem(new Guid("54d54c4e-d774-4b01-af87-0d0b94510767"), new Guid("6b85f863-7991-4f93-bf86-8c756fdeac87"), "Fall of Civilizations: Stories of Greatness and Decline", Helpers.Enums.ProductType.Book, 2, 14.39m),
            CreateOrderItem(new Guid("07a2ab81-7651-484f-945e-08074e7662bc"), new Guid("285c81bc-f257-4ffb-b6ce-7ab5fa9e5c81"), "Skandar and the Chaos Trials", Helpers.Enums.ProductType.Book, 1, 12.99m),
            CreateOrderItem(new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"), new Guid("01f54aa7-c51a-4b92-a72b-68e0965bf246"), "Funny Story", Helpers.Enums.ProductType.Book, 1, 11.99m),
            CreateOrderItem(new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"), new Guid("ecf65c56-5670-473b-9f20-fb0b191c2f0f"), "Saltblood", Helpers.Enums.ProductType.Book, 3, 15.99m),
            CreateOrderItem(new Guid("69e3878e-8293-4e81-8791-31328e2a3907"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", Helpers.Enums.ProductType.Book, 1, 7.50m),
            CreateOrderItem(new Guid("69e3878e-8293-4e81-8791-31328e2a3907"), new Guid("f3fcab1f-1c11-47f5-9e11-7868a88408e6"), "Thunderhead", Helpers.Enums.ProductType.Book, 1, 2.50m),
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