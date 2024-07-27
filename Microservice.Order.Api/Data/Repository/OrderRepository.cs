using Microservice.Order.Api.Data.Contexts;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.Api.Data.Repository;

public class OrderRepository(IDbContextFactory<OrderDbContext> dbContextFactory) : IOrderRepository
{    
    public IDbContextFactory<OrderDbContext> _dbContextFactory { get; set; } = dbContextFactory;

    public async Task<Domain.Order> AddAsync(Domain.Order order)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        await db.AddAsync(order);
        db.SaveChanges(); 

        return order;
    }

    public async Task UpdateAsync(Domain.Order order)
    {
        using var db = _dbContextFactory.CreateDbContext();

        db.Entry(order).State = EntityState.Modified; 
        await db.SaveChangesAsync(); 
    }

    public async Task Delete(Domain.Order order)
    { 
        using var db = _dbContextFactory.CreateDbContext(); 
         
        db.Orders.Remove(order);
        await db.SaveChangesAsync(); 
    }

    public async Task<Domain.Order> GetByIdAsync(Guid id, Guid customerId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Orders
                        .Where(o => o.Id.Equals(id) && o.CustomerId.Equals(customerId))
                        .Include(e => e.OrderItems)
                        .Include("OrderItems.ProductType")
                        .Include(e => e.OrderStatus)
                        .SingleOrDefaultAsync();
    }

    public async Task<List<Domain.Order>> SearchByDateAsync(DateOnly date)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Orders
                        .Where(o => o.Created.Equals(date))
                        .Include(e => e.OrderItems)
                        .Include(e => e.OrderStatus)
                        .OrderBy(e => e.Created)
                        .ToListAsync();
    } 

    public async Task<Domain.Order> OrderSummaryReadOnlyAsync(Guid orderId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();     
        return await db.Orders.AsNoTracking()
                        .Where(o => o.Id.Equals(orderId))
                        .Include(e => e.OrderItems)
                        .Include("OrderItems.ProductType")
                        .Include(e => e.OrderStatus) 
                        .SingleOrDefaultAsync();
    }

    public async Task<Boolean> OrderNotFound(Guid orderId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return db.Orders.AsNoTracking()
                        .Where(o => o.Id.Equals(orderId)).Count().Equals(0);
    }

    public async Task<Boolean> OrderIsClosedAsync(Guid orderId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return db.Orders.AsNoTracking()
                        .Where(o => o.Id.Equals(orderId) && o.OrderStatusId.Equals(Enums.OrderStatus.Completed))                         
                        .Count().Equals(1);
    }    
}