// Ignore Spelling: Mediatr

using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Domain;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.MediatR.GetOrder;
using Microservice.Order.Api.Protos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Order.Api.Test.Unit.Mediatr;

[TestFixture]
public class GetOrderMediatrTests
{
    private readonly Mock<ICustomerAddressService> customerAddressGrpcService = new();
    private readonly Mock<IOrderRepository> orderRepositoryMock = new();
    private readonly Mock<ILogger<GetOrderQueryHandler>> loggerMock = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetOrderQueryHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<IOrderRepository>(sp => orderRepositoryMock.Object);
        services.AddScoped<ICustomerAddressService>(sp => customerAddressGrpcService.Object);
        services.AddScoped<ILogger<GetOrderQueryHandler>>(sp => loggerMock.Object);
        services.AddScoped<IOrderHelper, OrderHelper>();
        services.AddAutoMapper(Assembly.GetAssembly(typeof(GetOrderMapper)));

        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [Test]
    public async Task Get_order_successfully_return_order()
    {
        Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");
        Guid customerId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");

        var orderItem1 = new Api.Domain.OrderItem
        {
            OrderId = orderId,
            ProductId = new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"),
            Name = "Infinity Kings",
            ProductType = new Api.Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 9.99m
        };

        var orderItem2 = new Api.Domain.OrderItem
        {
            OrderId = orderId,
            ProductId = new Guid("6131ce7e-fb11-4608-a3d3-f01caee2c465"),
            Name = "Infinity Reaper",
            ProductType = new Api.Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 8.99m
        };

        var orderItems = new List<OrderItem>() { orderItem1, orderItem2 };

        var order = new Api.Domain.Order
        {
            Id = orderId,
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressSurname = "Test",
            AddressForename = "Jake",
            OrderNumber = 1,
            OrderItems = orderItems,
            OrderStatusId = Enums.OrderStatus.Created,
            OrderStatus = new OrderStatus() { Id = Enums.OrderStatus.Created, Status = "Created" },
            Total = 18.98m
        };

        var customerAddress = new CustomerAddressResponse
        {
            AddressLine1 = "AddressLine1",
            AddressLine2 = "AddressLine2",
            AddressLine3 = "AddressLine3",
            TownCity = "TownCity",
            County = "County",
            Postcode = "Postcode",
            Country = "Country"
        };

        orderRepositoryMock
                .Setup(x => x.OrderSummaryReadOnlyAsync(orderId))
                .Returns(Task.FromResult(order));

        customerAddressGrpcService
                .Setup(x => x.GetCustomerAddressAsync(customerAddressId))
                .Returns(Task.FromResult(customerAddress));

        var getOrderRequest = new GetOrderRequest(orderId);
        var actualResult = await mediator.Send(getOrderRequest);

        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Id, Is.EqualTo(orderId));
            Assert.That(actualResult.OrderItems, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void Get_order_not_found_return_exception()
    {
        Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");

        orderRepositoryMock
                .Setup(x => x.OrderSummaryReadOnlyAsync(orderId));

        var getOrderRequest = new GetOrderRequest(orderId);

        var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await mediator.Send(getOrderRequest);
        });

        Assert.That(validationException.Message, Is.EqualTo($"Order not found for order."));
    }

    [Test]
    public void Get_order_customer_address_not_found_return_exception()
    {
        Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");
        Guid customerId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");

        var orderItem1 = new Api.Domain.OrderItem
        {
            OrderId = orderId,
            ProductId = new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"),
            Name = "Infinity Kings",
            ProductType = new Api.Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 9.99m
        };

        var orderItem2 = new Api.Domain.OrderItem
        {
            OrderId = orderId,
            ProductId = new Guid("6131ce7e-fb11-4608-a3d3-f01caee2c465"),
            Name = "Infinity Reaper",
            ProductType = new Api.Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 8.99m
        };

        var orderItems = new List<OrderItem>() { orderItem1, orderItem2 };

        var order = new Api.Domain.Order
        {
            Id = orderId,
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressSurname = "Test",
            AddressForename = "Jake",
            OrderNumber = 1,
            OrderItems = orderItems,
            OrderStatusId = Enums.OrderStatus.Created,
            OrderStatus = new OrderStatus() { Id = Enums.OrderStatus.Created, Status = "Created" },
            Total = 18.98m
        };

        orderRepositoryMock
                .Setup(x => x.OrderSummaryReadOnlyAsync(orderId))
                .Returns(Task.FromResult(order));

        customerAddressGrpcService
                .Setup(x => x.GetCustomerAddressAsync(customerAddressId));

        var getOrderRequest = new GetOrderRequest(orderId);

        var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await mediator.Send(getOrderRequest);
        });

        Assert.That(validationException.Message, Is.EqualTo($"Customer address not found for order."));
    }
}