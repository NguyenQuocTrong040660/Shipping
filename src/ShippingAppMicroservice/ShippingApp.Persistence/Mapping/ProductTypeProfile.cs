using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class ProductTypeProfile: Profile
    {
        public ProductTypeProfile()
        { 
            CreateMap<Entities.ProductType, Models.ProductType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductTypeName))
                .ReverseMap();
        }
    }
}
