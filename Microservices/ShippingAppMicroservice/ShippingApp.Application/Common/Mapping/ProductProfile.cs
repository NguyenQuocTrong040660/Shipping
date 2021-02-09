using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;
using AutoMapper;

namespace ShippingApp.Application.Common.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Entities.Product, Models.ProductModel>()
                .ReverseMap();
        }
    }
}
