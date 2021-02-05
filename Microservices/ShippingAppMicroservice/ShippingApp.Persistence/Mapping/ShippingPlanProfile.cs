using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class ShippingPlanProfile : Profile
    {
        public ShippingPlanProfile()
        {
            CreateMap<Entities.ShippingPlan, Models.ShippingPlan>()
                .ReverseMap();

            CreateMap<Models.ShippingPlan, DTO.ShippingPlan>()
                .ReverseMap();

        }
    }
}
