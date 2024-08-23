using AutoMapper;
using Microservice.Order.Api.Helpers;

namespace Microservice.Order.Api.MediatR.AddOrder;

public class AddOrderMapper : Profile
{
    public AddOrderMapper()
    {
        base.CreateMap<AddOrderItemRequest, Microservice.Order.Api.Domain.OrderItem>()
            .ForMember(m => m.ProductId, o => o.MapFrom(s => s.Id))
            .ForMember(m => m.Name, o => o.MapFrom(s => ""))
            .ForMember(m => m.ProductTypeId, o => o.MapFrom(s => s.ProductType))
            .ForMember(m => m.ProductType, opt => opt.Ignore())
            .ForMember(m => m.Quantity, o => o.MapFrom(s => s.Quantity));

        base.CreateMap<AddOrderRequest, Microservice.Order.Api.Domain.Order>()
            .ForMember(m => m.CustomerId, o => o.MapFrom(s => s.CustomerId))
            .ForMember(m => m.CustomerAddressId, o => o.MapFrom(s => s.CustomerAddressId))
            .ForMember(m => m.OrderItems, o => o.MapFrom(s => s.OrderItems))
            .ForMember(m => m.OrderStatusId, o => o.MapFrom(s => Enums.OrderStatus.Created))
            .ForMember(m => m.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

        base.CreateMap<Domain.OrderItem, AddOrderInvalidOrderItemResponse>()
            .ForMember(m => m.ProductId, o => o.MapFrom(s => s.ProductId))
            .ForMember(m => m.Quantity, o => o.MapFrom(s => s.Quantity));

        base.CreateMap<Api.Domain.OrderItem, AddOrderOrderItemResponse>()
            .ForCtorParam(nameof(AddOrderOrderItemResponse.ProductType), opt => opt.MapFrom(src => src.ProductTypeId));

        base.CreateMap<Protos.CustomerAddressResponse, AddOrderAddressResponse>();
    }
}