using AutoMapper;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Mediatr.CompletedOrder.Model;

namespace Microservice.Order.Api.Mediatr.CompletedOrder;

public class CompletedOrderMapper : Profile
{
    public CompletedOrderMapper()
    { 
        base.CreateMap<Domain.OrderItem, OrderItemHistory>()
            .ForMember(m => m.ProductId, o => o.MapFrom(s => s.ProductId))
            .ForMember(m => m.Name, o => o.MapFrom(s => s.Name))
            .ForMember(m => m.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(m => m.Quantity, o => o.MapFrom(s => s.Quantity))
            .ForMember(m => m.UnitPrice, o => o.MapFrom(s => s.UnitPrice));
         
        base.CreateMap<Domain.Order, OrderHistory>()
            .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
            .ForMember(m => m.CustomerId, o => o.MapFrom(s => s.CustomerId))
            .ForMember(m => m.AddressSurname, o => o.MapFrom(s => s.AddressSurname))
            .ForMember(m => m.AddressForename, o => o.MapFrom(s => s.AddressForename))
            .ForMember(m => m.OrderNumber, o => o.MapFrom(s => OrderHelper.PaddedOrderNumber(s.OrderNumber)))
            .ForMember(m => m.OrderItems, o => o.MapFrom(s => s.OrderItems))
            .ForMember(m => m.OrderStatus, o => o.MapFrom(s => s.OrderStatus.Status))
            .ForMember(m => m.Total, o => o.MapFrom(s => s.Total))
            .ForMember(m => m.OrderPlaced, o => o.MapFrom(s => DateOnly.FromDateTime(s.Created)));
    }
}
