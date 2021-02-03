using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        { 
            CreateMap<Entities.Reservation, Models.Reservation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
                .ForMember(dest => dest.DateSet, opt => opt.MapFrom(src => src.DateSet))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CountPerson, opt => opt.MapFrom(src => src.CountPerson))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName))
                .ReverseMap();
        }
    }
}
