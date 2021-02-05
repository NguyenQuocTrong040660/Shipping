using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Entities.ProductEntity, Models.ProductModel>()
                .ReverseMap();

            CreateMap<Models.ProductModel, DTO.ProductDTO>()
                .ReverseMap();

        }
    }
}
