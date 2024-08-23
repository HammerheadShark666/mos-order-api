using AutoMapper;

namespace Microservice.Order.Api.MediatR.GetOrder;

public class GetOrderMapper : Profile
{
    public GetOrderMapper()
    {
        base.CreateMap<Api.Domain.OrderItem, GetOrderOrderItemResponse>()
            .ForCtorParam(nameof(GetOrderOrderItemResponse.Type),
                    opt => opt.MapFrom(src => src.ProductType.Name));

        base.CreateMap<Protos.CustomerAddressResponse, GetOrderAddressResponse>();
    }
}