using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using AutoMapper;

namespace ShippingApp.Application.Common.Mapping
{
    public class ProductTypeProfile: Profile
    {
        public ProductTypeProfile()
        { 
            CreateMap<Entities.ProductType, Models.ProductTypeModel>()
                .ReverseMap();
        }
    }
}
