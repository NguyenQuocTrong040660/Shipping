using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;

namespace ShippingApp.Persistence.Mapping
{
    public class CountryProfile: Profile
    {
        public CountryProfile()
        {
            CreateMap<Entities.Country, Models.Country>().ReverseMap();
        }
    }
}
