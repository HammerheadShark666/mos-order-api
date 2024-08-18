﻿using FluentValidation;
using Grpc.Core;
using Microservice.Order.Api.Helpers.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Microservice.Order.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (RpcException e)
        {
            _logger.LogError(e, e.Message); 
            await HandleRpcExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message); 
            await HandleExceptionAsync(context, e);
        } 
    }

    private static async Task HandleRpcExceptionAsync(HttpContext httpContext, RpcException exception)
    {
        httpContext.Response.StatusCode = (int)exception.Status.StatusCode;

        var response = new
        {
            status = exception.Status.StatusCode.ToString(),
            detail = exception.Status.Detail
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    } 

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {  
        var statusCode = GetStatusCode(exception);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        switch (statusCode)
        {
            case 404:
                {
                    var response = new
                    {
                        status = statusCode,
                        detail = GetMessage(exception)
                    };

                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
                    break;
                }
            default:
                {
                    var response = new
                    {
                        status = statusCode,
                        detail = GetMessage(exception),
                        errors = GetErrors(exception)
                    };

                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
                    break;
                }
        }
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        }; 

    private static string GetMessage(Exception exception) =>
        exception switch
        {
            ValidationException => "Validation Error",
            _ => exception.Message
        };  
     
    private static IEnumerable<string> GetErrors(Exception exception)
    { 
        if (exception is ValidationException validationException)
        {  
            foreach (var  error in validationException.Errors)
            {
                yield return error.ErrorMessage;
            }
        }
    }
}