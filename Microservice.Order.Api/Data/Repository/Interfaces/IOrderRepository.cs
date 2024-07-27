namespace Microservice.Order.Api.Data.Repository.Interfaces;

public interface IOrderRepository
{
    Task<Domain.Order> AddAsync(Domain.Order order);
    Task UpdateAsync(Domain.Order entity);
    Task Delete(Domain.Order order);
    Task<Domain.Order> GetByIdAsync(Guid id, Guid customerId);
    Task<Domain.Order> OrderSummaryReadOnlyAsync(Guid id);    
    Task<List<Domain.Order>> SearchByDateAsync(DateOnly date);
    Task<Boolean> OrderNotFound(Guid orderId);
    Task<Boolean> OrderIsClosedAsync(Guid orderId);
}