// Ignore Spelling: Mediatr

using FluentValidation;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Domain;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.MediatR.CompletedOrder;
using Microservice.Order.Api.Protos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Order.Api.Test.Unit.Mediatr;

[TestFixture]
public class CompletedOrderMediatrTests
{
    private readonly Mock<ICustomerAddressService> customerAddressGrpcService = new();
    private readonly Mock<IOrderRepository> orderRepositoryMock = new();
    private readonly Mock<IAzureServiceBusHelper> azureServiceBusHelperMock = new();
    private readonly Mock<ICustomerHttpAccessor> customerHttpAccessorMock = new();
    private readonly Mock<ILogger<CompletedOrderCommandHandler>> loggerMock = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<CompletedOrderValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CompletedOrderCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<IOrderRepository>(sp => orderRepositoryMock.Object);
        services.AddScoped<ICustomerAddressService>(sp => customerAddressGrpcService.Object);
        services.AddScoped<IAzureServiceBusHelper>(sp => azureServiceBusHelperMock.Object);
        services.AddScoped<ICustomerHttpAccessor>(sp => customerHttpAccessorMock.Object);
        services.AddScoped<ILogger<CompletedOrderCommandHandler>>(sp => loggerMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CompletedOrderMapper)));

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
    public async Task Order_completed_successfully_return_message()
    {
        Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");
        Guid customerId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");

        var orderItem1 = new Api.Domain.OrderItem
        {
            OrderId = orderId,
            ProductId = new Guid("6131ce7e-fb11-4608-a3d3-f01caee2c465"),
            Name = "Infinity Reaper",
            ProductType = new Api.Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 8.99m
        };

        var orderItems = new List<OrderItem>() { orderItem1 };

        var order = new Api.Domain.Order()
        {
            Id = orderId,
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressForename = "",
            AddressSurname = "",
            OrderNumber = 1,
            OrderItems = orderItems,
            Total = 8.99m,
            OrderStatusId = Enums.OrderStatus.Dispatched
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

        customerAddressGrpcService
                .Setup(x => x.GetCustomerAddressAsync(customerAddressId))
                .Returns(Task.FromResult(customerAddress));

        orderRepositoryMock
                .Setup(x => x.Exists(orderId))
                .Returns(Task.FromResult(true));

        orderRepositoryMock
                .Setup(x => x.OrderIsClosedAsync(orderId))
                .Returns(Task.FromResult(false));

        orderRepositoryMock
                .Setup(x => x.GetByIdAsync(orderId, customerId))
                .Returns(Task.FromResult(order));

        orderRepositoryMock
                .Setup(x => x.UpdateAsync(order));

        customerHttpAccessorMock.Setup(x => x.CustomerId)
                                .Returns(customerId);

        azureServiceBusHelperMock.Setup(x => x.SendMessage(EnvironmentVariables.AzureServiceBusQueueOrderCompleted, It.IsNotNull<string>()));

        var completedOrderRequest = new CompletedOrderRequest(orderId);
        var actualResult = await mediator.Send(completedOrderRequest);

        Assert.That(actualResult.Message, Is.EqualTo("Order completed."));
    }

    //[Test]
    //public void Complete_order_customer_not_found_return_exception()
    //{
    //    Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");
    //    Guid customerId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");

    //    var completedOrderRequest = new CompletedOrderRequest(orderId);
    //    var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
    //    {
    //        await mediator.Send(completedOrderRequest);
    //    });

    //    Assert.That(validationException.Message, Is.EqualTo($"Customer not found."));
    //}

    //[Test]
    //public void Complete_order_order_not_found_return_exception()
    //{
    //    Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");
    //    Guid customerId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");

    //    customerHttpAccessorMock.Setup(x => x.CustomerId)
    //                            .Returns(customerId);

    //    orderRepositoryMock
    //            .Setup(x => x.GetByIdAsync(orderId, customerId))
    //            .ReturnsAsync((Domain.Order)null);

    //    var completedOrderRequest = new CompletedOrderRequest(orderId);
    //    var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
    //    {
    //        await mediator.Send(completedOrderRequest);
    //    });

    //    Assert.That(validationException.Message, Is.EqualTo($"Order not found for order - {orderId.ToString()}"));
    //}

    [Test]
    public void Order_completed_customer_address_not_found_return_message()
    {
        Guid orderId = new("07c06c3f-0897-44b6-ae05-a70540e73a12");
        Guid customerId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");

        var orderItem1 = new Api.Domain.OrderItem
        {
            OrderId = orderId,
            ProductId = new Guid("6131ce7e-fb11-4608-a3d3-f01caee2c465"),
            Name = "Infinity Reaper",
            ProductType = new Api.Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 8.99m
        };

        var orderItems = new List<OrderItem>() { orderItem1 };

        var order = new Api.Domain.Order()
        {
            Id = orderId,
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressForename = "",
            AddressSurname = "",
            OrderNumber = 1,
            OrderItems = orderItems,
            Total = 8.99m,
            OrderStatusId = Enums.OrderStatus.Dispatched
        };

        customerAddressGrpcService
                .Setup(x => x.GetCustomerAddressAsync(customerAddressId));

        orderRepositoryMock
                .Setup(x => x.Exists(orderId))
                .Returns(Task.FromResult(true));

        orderRepositoryMock
                .Setup(x => x.OrderIsClosedAsync(orderId))
                .Returns(Task.FromResult(false));

        orderRepositoryMock
                .Setup(x => x.GetByIdAsync(orderId, customerId))
                .Returns(Task.FromResult(order));

        customerHttpAccessorMock.Setup(x => x.CustomerId)
                                .Returns(customerId);

        azureServiceBusHelperMock.Setup(x => x.SendMessage(EnvironmentVariables.AzureServiceBusQueueOrderCompleted, It.IsNotNull<string>()));

        var completedOrderRequest = new CompletedOrderRequest(orderId);
        var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await mediator.Send(completedOrderRequest);
        });

        Assert.That(validationException.Message, Is.EqualTo($"Customer address not found for order id."));
    }
}