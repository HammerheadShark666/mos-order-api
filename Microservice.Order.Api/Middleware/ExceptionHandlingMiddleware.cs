using FluentValidation;
using FluentValidation.Results;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using System.Net;
using System.Text.Json;
using static Microservice.Order.Api.Helpers.Enums;

namespace Microservice.Order.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{message}", e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var exceptionResults = GetExceptionResults(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exceptionResults.Item1;

        return context.Response.WriteAsync(exceptionResults.Item2);
    }

    private static IEnumerable<ResponseMessage> GetValidationErrors(IEnumerable<ValidationFailure> validationErrors)
    {
        if (validationErrors != null)
        {
            foreach (var error in validationErrors)
            {
                yield return new ResponseMessage(ErrorType.Error.ToString(), error.ErrorMessage);
            }
        }
    }

    private static ResponseMessage CreateValidationError(string type, string message)
    {
        return new ResponseMessage(type, message);
    }

    private static Tuple<HttpStatusCode, string> GetExceptionResults(Exception exception)
    {
        var httpStatusCode = HttpStatusCode.BadRequest;
        var errorMessages = JsonSerializer.Serialize(CreateValidationError(Enums.ErrorType.Error.ToString(), exception.Message));

        if (exception.InnerException != null)
        {
            exception = exception.InnerException;
            errorMessages = JsonSerializer.Serialize(CreateValidationError(Enums.ErrorType.Error.ToString(), exception.Message));
        }

        switch (exception)
        {
            case BadRequestException:
            case ArgumentException:
            case EnvironmentVariableNotFoundException:
                break;
            case ValidationException validationException:
                errorMessages = JsonSerializer.Serialize(GetValidationErrors(validationException.Errors));
                break;
            case NotFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case not null:
                httpStatusCode = HttpStatusCode.InternalServerError;
                break;
        }

        return new Tuple<HttpStatusCode, string>(httpStatusCode, errorMessages);
    }
}

//internal sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
//{
//    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

//    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    {
//        try
//        {
//            await next(context);
//        }
//        catch (Exception e)
//        {
//            _logger.LogError(e, "{e.Message}", e.Message);
//            await HandleExceptionAsync(context, e);
//        }
//    }

//    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
//    {
//        var httpStatusCode = HttpStatusCode.InternalServerError;

//        context.Response.ContentType = "application/json";

//        var result = string.Empty;

//        switch (exception)
//        {
//            case ValidationException validationException:
//                httpStatusCode = HttpStatusCode.BadRequest;
//                result = JsonSerializer.Serialize(GetValidationErrors(validationException.Errors));
//                break;
//            case ArgumentException argumentException:
//                httpStatusCode = HttpStatusCode.BadRequest;
//                result = JsonSerializer.Serialize(argumentException.Message);
//                break;
//            case BadRequestException badRequestException:
//                httpStatusCode = HttpStatusCode.BadRequest;
//                result = badRequestException.Message;
//                break;
//            case NotFoundException:
//                httpStatusCode = HttpStatusCode.NotFound;
//                break;
//            case not null:
//                httpStatusCode = HttpStatusCode.BadRequest;
//                break;
//        }

//        context.Response.StatusCode = (int)httpStatusCode;

//        if (result == string.Empty) result = JsonSerializer.Serialize(new { error = exception?.Message });

//        return context.Response.WriteAsync(result);
//    }

//    private static IEnumerable<Helpers.ResponseMessage> GetValidationErrors(IEnumerable<ValidationFailure> validationErrors)
//    {
//        if (validationErrors != null)
//        {
//            foreach (var error in validationErrors)
//            {
//                yield return new Helpers.ResponseMessage(ErrorType.Error.ToString(), error.ErrorMessage);
//            }
//        }
//    }
//}