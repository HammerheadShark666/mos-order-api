using Microservice.Order.Api.Protos;

namespace Microservice.Order.Api.Grpc.Interfaces;

public interface IBookService
{
    Task<BooksResponse> GetBooksDetailsAsync(List<Guid> productIds);
}