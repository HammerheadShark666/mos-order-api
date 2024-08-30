using Grpc.Core;
using Grpc.Net.Client;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.Protos;

namespace Microservice.Order.Api.Grpc;

public class BookService(IJwtHelper jwtHelper) : IBookService
{
    public record BookDetailResponse(Guid Id, string Name, decimal UnitPrice);
    public record NotFoundBookDetailsResponse(Guid Id);
    public record BookDetailsResponse(List<BookDetailResponse> BookDetailResponse, List<NotFoundBookDetailsResponse> NotFoundBookDetailsResponse);

    public async Task<BooksResponse> GetBooksDetailsAsync(List<Guid> productIds)
    {
        ILoggerFactory _loggerFactory = LoggerFactory.Create(b => b.AddConsole());
        ILogger _logger = _loggerFactory.CreateLogger<BookService>();

        using var channel = GrpcChannel.ForAddress(EnvironmentVariables.GRPC_Book_Url, new GrpcChannelOptions { HttpHandler = GetClientHandler() });
        var client = new BookGrpc.BookGrpcClient(channel);
        var response = await client.GetBooksAsync(GetListBookRequest(productIds), GetGrpcHeaders());

        _logger.LogInformation("Books grpc call completed.");
        _logger.LogInformation("Books not found: {bookIds} ", String.Join(", ", response.NotFoundBookResponses));

        return response;
    }

    private Metadata GetGrpcHeaders()
    {
        var headers = new Metadata
        {
            { "Authorization", $"Bearer {jwtHelper.GenerateJwtToken()}" }
        };

        return headers;
    }

    private static HttpClientHandler GetClientHandler()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        return handler;
    }

    private static ListBookRequest GetListBookRequest(List<Guid> productIds)
    {
        List<BookRequest> bookRequests = [];

        foreach (var productId in productIds)
        {
            bookRequests.Add(new BookRequest() { Id = productId.ToString() });
        }

        return new ListBookRequest()
        {
            BookRequests = { bookRequests }
        };
    }
}