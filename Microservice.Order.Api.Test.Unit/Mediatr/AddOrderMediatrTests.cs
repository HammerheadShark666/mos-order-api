using FluentValidation;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Domain;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.MediatR.AddOrder;
using Microservice.Order.Api.Protos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Order.Api.Test.Unit;

[TestFixture]
public class AddOrderMediatrTests
{
    private Mock<ICustomerAddressService> customerAddressGrpcServiceMock = new();
    private Mock<IBookService> bookGrpcServiceMock = new();
    private Mock<IOrderRepository> orderRepositoryMock = new();
    private Mock<ILogger<AddOrderCommandHandler>> loggerMock = new();
    private ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<AddOrderValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddOrderCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<IOrderRepository>(sp => orderRepositoryMock.Object);
        services.AddScoped<ICustomerAddressService>(sp => customerAddressGrpcServiceMock.Object);
        services.AddScoped<IBookService>(sp => bookGrpcServiceMock.Object);
        services.AddScoped<ILogger<AddOrderCommandHandler>>(sp => loggerMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddOrderMapper)));

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
    public async Task Add_order_successfully_return_order()
    {
        Guid customerId = new("6c84d0a3-0c0c-435f-9ae0-4de09247ee15");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");
        Guid bookId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");

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

        customerAddressGrpcServiceMock
             .Setup(x => x.GetCustomerAddressAsync(customerAddressId))
             .Returns(Task.FromResult(customerAddress));

        var books = new List<Guid>() { bookId };
        var booksResponse = new BooksResponse();
        booksResponse.BookResponses.Add(new BookResponse() { Id = "29a75938-ce2d-473b-b7fe-2903fe97fd6e", Name = "Infinity Kings", UnitPrice = "8.99" });

        bookGrpcServiceMock
                .Setup(x => x.GetBooksDetailsAsync(books))
                .Returns(Task.FromResult(booksResponse));

        var orderCreatedOrderItems = new List<OrderItem>() { new() {
            ProductId = new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"),
            Name = "Infinity Kings",
            ProductType = new Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 9.99m
        }};

        var orderCreated = new Domain.Order
        {
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressSurname = "Test",
            AddressForename = "Jake",
            OrderNumber = 1,
            OrderItems = orderCreatedOrderItems,
            OrderStatusId = Enums.OrderStatus.Created,
            OrderStatus = new OrderStatus() { Id = Enums.OrderStatus.Created, Status = "Created" },
            Total = 18.98m
        };

        orderRepositoryMock.Setup(x => x.AddAsync(orderCreated));

        var orderItem = new AddOrderItemRequest(new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), 1, Enums.ProductType.Book);
        var orderItems = new List<AddOrderItemRequest>();
        orderItems.Add(orderItem);

        var addOrderRequest = new AddOrderRequest(customerId, customerAddressId, "", "", orderItems);
        var actualResult = await mediator.Send(addOrderRequest);

        Assert.IsInstanceOf(typeof(Guid), actualResult.order.Id);
        Assert.That(actualResult.order.OrderItems.Count, Is.EqualTo(1));
        Assert.That(actualResult.invalidOrderItems.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task Add_order_successfully_one_order_item_not_valid_return_order()
    {
        Guid customerId = new("6c84d0a3-0c0c-435f-9ae0-4de09247ee15");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");
        Guid bookId1 = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");
        Guid bookId2 = new("07c06c3f-0897-44b6-ae05-a70540e73a12");

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

        customerAddressGrpcServiceMock
             .Setup(x => x.GetCustomerAddressAsync(customerAddressId))
             .Returns(Task.FromResult(customerAddress));

        var books = new List<Guid>() { bookId1, bookId2 };
        var booksResponse = new BooksResponse();
        booksResponse.BookResponses.Add(new BookResponse() { Id = "29a75938-ce2d-473b-b7fe-2903fe97fd6e", Name = "Infinity Kings", UnitPrice = "8.99" });
        booksResponse.NotFoundBookResponses.Add(new NotFoundBookResponse() { Id = bookId2.ToString() });

        bookGrpcServiceMock
                .Setup(x => x.GetBooksDetailsAsync(books))
                .Returns(Task.FromResult(booksResponse));

        var orderCreatedOrderItems = new List<OrderItem>() { new() {
            ProductId = new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"),
            Name = "Infinity Kings",
            ProductType = new Domain.ProductType() { Id = Enums.ProductType.Book, Name = "Book" },
            Quantity = 1,
            UnitPrice = 9.99m
        }};

        var orderCreated = new Domain.Order
        {
            CustomerId = customerId,
            CustomerAddressId = customerAddressId,
            AddressSurname = "Test",
            AddressForename = "Jake",
            OrderNumber = 1,
            OrderItems = orderCreatedOrderItems,
            OrderStatusId = Enums.OrderStatus.Created,
            OrderStatus = new OrderStatus() { Id = Enums.OrderStatus.Created, Status = "Created" },
            Total = 18.98m
        };

        orderRepositoryMock.Setup(x => x.AddAsync(orderCreated));

        var orderItemA = new AddOrderItemRequest(new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), 1, Enums.ProductType.Book);
        var orderItemB = new AddOrderItemRequest(new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), 1, Enums.ProductType.Book);
        var orderItems = new List<AddOrderItemRequest>();
        orderItems.Add(orderItemA);
        orderItems.Add(orderItemB);

        var addOrderRequest = new AddOrderRequest(customerId, customerAddressId, "", "", orderItems);
        var actualResult = await mediator.Send(addOrderRequest);

        Assert.IsInstanceOf(typeof(Guid), actualResult.order.Id);
        Assert.That(actualResult.order.OrderItems.Count, Is.EqualTo(1));
        Assert.That(actualResult.order.OrderItems[0].Name, Is.EqualTo("Infinity Kings"));

        Assert.That(actualResult.invalidOrderItems.Count, Is.EqualTo(1));
        Assert.That(actualResult.invalidOrderItems[0].ProductId, Is.EqualTo(bookId2));
    }

    [Test]
    public void Add_order_fail_no_order_items_return_message()
    {
        Guid customerId = new("6c84d0a3-0c0c-435f-9ae0-4de09247ee15");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");

        var orderItems = new List<AddOrderItemRequest>();

        var addOrderRequest = new AddOrderRequest(customerId, customerAddressId, "", "", orderItems);
        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addOrderRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(1));
        Assert.That(validationException.Errors.First().ErrorMessage, Is.EqualTo($"Order has no order items."));
    }

    [Test]
    public void Add_order_fail_no_customer_address_return_message()
    {
        Guid customerId = new("6c84d0a3-0c0c-435f-9ae0-4de09247ee15");
        Guid customerAddressId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");
        Guid bookId1 = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");
        Guid bookId2 = new("07c06c3f-0897-44b6-ae05-a70540e73a12");

        customerAddressGrpcServiceMock
             .Setup(x => x.GetCustomerAddressAsync(customerAddressId));

        var books = new List<Guid>() { bookId1, bookId2 };
        var booksResponse = new BooksResponse();
        booksResponse.BookResponses.Add(new BookResponse() { Id = "29a75938-ce2d-473b-b7fe-2903fe97fd6e", Name = "Infinity Kings", UnitPrice = "8.99" });
        booksResponse.NotFoundBookResponses.Add(new NotFoundBookResponse() { Id = bookId2.ToString() });

        bookGrpcServiceMock
                .Setup(x => x.GetBooksDetailsAsync(books))
                .Returns(Task.FromResult(booksResponse));

        var orderItemA = new AddOrderItemRequest(new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), 1, Enums.ProductType.Book);
        var orderItemB = new AddOrderItemRequest(new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), 1, Enums.ProductType.Book);
        var orderItems = new List<AddOrderItemRequest>();
        orderItems.Add(orderItemA);
        orderItems.Add(orderItemB);

        var addOrderRequest = new AddOrderRequest(customerId, customerAddressId, "", "", orderItems);

        var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await mediator.Send(addOrderRequest);
        });

        Assert.That(validationException.Message, Is.EqualTo("Customer address not found."));
    }
}