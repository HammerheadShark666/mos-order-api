using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.Api.Data.Context;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Order> Orders { get; set; }
    public DbSet<Domain.OrderItem> OrderItems { get; set; }
    public DbSet<Domain.OrderStatus> OrderStatus { get; set; }
    public DbSet<Domain.ProductType> ProductType { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasSequence<int>("OrderNumberCounter");

        modelBuilder.Entity<Domain.Order>()
                        .Property(o => o.OrderNumber)
                        .HasDefaultValueSql("NEXT VALUE FOR OrderNumberCounter");

        modelBuilder.Entity<Domain.Order>().HasMany(e => e.OrderItems);
        modelBuilder.Entity<Domain.OrderItem>().HasKey(e => new { e.OrderId, e.ProductId });

        modelBuilder.Entity<Domain.ProductType>().HasData(DefaultData.GetProductTypeDefaultData());
        modelBuilder.Entity<Domain.OrderStatus>().HasData(DefaultData.GetOrderStatusDefaultData());
        modelBuilder.Entity<Domain.Order>().HasData(DefaultData.GetOrderDefaultData());
        modelBuilder.Entity<Domain.OrderItem>().HasData(DefaultData.GetOrderItemDefaultData());
    }
}