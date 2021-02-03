using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class MemberShipProfile : Profile
    {
        public MemberShipProfile()
        { 
            CreateMap<Entities.MemberShip, Models.MemberShip>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
        }
    }
}
