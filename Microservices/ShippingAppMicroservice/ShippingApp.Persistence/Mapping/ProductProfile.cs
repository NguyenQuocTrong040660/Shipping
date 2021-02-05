﻿using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;
using AutoMapper;

namespace ShippingApp.Persistence.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Entities.Product, Models.Product>()
                .ReverseMap();

            CreateMap<Models.Product, DTO.Product>()
                .ReverseMap();

        }
    }
}
