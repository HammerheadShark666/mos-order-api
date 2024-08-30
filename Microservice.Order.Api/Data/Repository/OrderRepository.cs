using Microservice.Order.Api.Data.Context;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.Api.Data.Repository;

public class OrderRepository(IDbContextFactory<OrderDbContext> dbContextFactory) : IOrderRepository
{
    public async Task<Domain.Order> AddAsync(Domain.Order order)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.AddAsync(order);
        db.SaveChanges();

        return order;
    }

    public async Task UpdateAsync(Domain.Order order)
    {
        using var db = dbContextFactory.CreateDbContext();

        db.Entry(order).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }

    public async Task<Domain.Order> GetByIdAsync(Guid id, Guid customerId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var order = await db.Orders
                        .Where(o => o.Id.Equals(id) && o.CustomerId.Equals(customerId))
                        .Include(e => e.OrderItems)
                        .Include("OrderItems.ProductType")
                        .Include(e => e.OrderStatus)
                        .SingleOrDefaultAsync() ?? throw new NotFoundException("Order not found.");
        return order;
    }

    public async Task<Domain.Order> OrderSummaryReadOnlyAsync(Guid orderId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var order = await db.Orders.AsNoTracking()
                        .Where(o => o.Id.Equals(orderId))
                        .Include(e => e.OrderItems)
                        .Include("OrderItems.ProductType")
                        .Include(e => e.OrderStatus)
                        .SingleOrDefaultAsync() ?? throw new NotFoundException("Order not found.");
        return order;
    }

    public async Task<Boolean> Exists(Guid orderId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return db.Orders.AsNoTracking()
                        .Where(o => o.Id.Equals(orderId)).Count().Equals(1);
    }

    public async Task<Boolean> OrderIsClosedAsync(Guid orderId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return db.Orders.AsNoTracking()
                        .Where(o => o.Id.Equals(orderId) && o.OrderStatusId.Equals(Enums.OrderStatus.Completed))
                        .Count().Equals(1);
    }
}