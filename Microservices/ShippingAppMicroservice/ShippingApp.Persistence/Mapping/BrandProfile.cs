using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;

namespace ShippingApp.Persistence.Mapping
{
    public class BrandProfile: Profile
    {
        public BrandProfile()
        {
            CreateMap<Entities.Brand, Models.Brand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
                .ReverseMap();
        }
    }
}
