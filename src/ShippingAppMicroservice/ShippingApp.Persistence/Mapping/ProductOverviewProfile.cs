using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class ProductOverviewProfile: Profile
    {
        public ProductOverviewProfile()
        {
            CreateMap<Entities.ProductOverview, Models.ProductOverview>()
                .ForMember(dest => dest.ProductTypeString, opt => opt.MapFrom(src => src.ProductType.ProductTypeName))
                .ForMember(dest => dest.CountryNameString, opt => opt.MapFrom(src => src.Country.CountryName))
                .ForMember(dest => dest.BrandNameString, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ReverseMap();
        }
    }
}
