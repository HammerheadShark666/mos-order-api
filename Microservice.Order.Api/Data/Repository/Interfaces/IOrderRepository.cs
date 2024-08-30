namespace Microservice.Order.Api.Data.Repository.Interfaces;

public interface IOrderRepository
{
    Task<Domain.Order> AddAsync(Domain.Order order);
    Task UpdateAsync(Domain.Order entity);
    Task<Domain.Order> GetByIdAsync(Guid id, Guid customerId);
    Task<Domain.Order> OrderSummaryReadOnlyAsync(Guid id);
    Task<Boolean> Exists(Guid orderId);
    Task<Boolean> OrderIsClosedAsync(Guid orderId);
}