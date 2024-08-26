using Asp.Versioning;
using MediatR;
using Microservice.Order.Api.Extensions;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.MediatR.AddOrder;
using Microservice.Order.Api.MediatR.CompletedOrder;
using Microservice.Order.Api.MediatR.GetOrder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Security.Claims;

namespace Microservice.Order.Api.Endpoints;

public static class Endpoints
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var orderGroup = webApplication.MapGroup("v{version:apiVersion}/orders").WithTags("orders");

        orderGroup.MapGet("/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async ([FromRoute] Guid id, [FromServices] IMediator mediator) =>
        {
            var getOrderResponse = await mediator.Send(new GetOrderRequest(id));
            return Results.Ok(getOrderResponse);
        })
        .Produces<GetOrderResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("GetOrder")
        .WithApiVersionSet(webApplication.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get a order based on id.",
            Description = "Gets a order based on its id.",
            Tags = [new() { Name = "Microservice Order System - Orders" }]
        });

        orderGroup.MapPost("/add", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (AddOrderRequest addOrderRequest, IMediator mediator, HttpContext http) =>
        {
            var customerId = http.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new NotFoundException("Customer not found");
            addOrderRequest = addOrderRequest with { CustomerId = new Guid(customerId) };

            var addOrderResponse = await mediator.Send(addOrderRequest);
            return Results.Ok(addOrderResponse);
        })
        .Accepts<AddOrderRequest>("application/json")
        .Produces<AddOrderResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("AddOrder")
        .WithApiVersionSet(webApplication.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add an order.",
            Description = "Adds an order.",
            Tags = [new() { Name = "Microservice Order System - Orders" }]
        });

        orderGroup.MapPost("/completed", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (CompletedOrderRequest completedOrderRequest, IMediator mediator, HttpContext http) =>
        {
            var completedOrderResponse = await mediator.Send(completedOrderRequest);
            return Results.Ok(completedOrderResponse);
        })
        .Accepts<CompletedOrderRequest>("application/json")
        .Produces<CompletedOrderResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("CompletedOrder")
        .WithApiVersionSet(webApplication.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Completes an order.",
            Description = "Completes an order.",
            Tags = [new() { Name = "Microservice Order System - Orders" }]
        });
    }
}