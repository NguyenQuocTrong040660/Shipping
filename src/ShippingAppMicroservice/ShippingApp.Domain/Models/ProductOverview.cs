using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class ProductOverview
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string HighLevelDesc { get; set; }
        public string MediumLevelDesc { get; set; }
        public string NormalLevelDesc { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public ProductGeneral ProductGeneral { get; set; }
        public ProductSpecification ProductSpecification { get; set; }
        public List<ProductDescription> ProductDescriptions { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public ProductType ProductType { get; set; }
        public Country Country { get; set; }
        public Brand Brand { get; set; }
        public string ProductTypeString { get; set; }
        public string CountryNameString { get; set; }
        public string BrandNameString { get; set; }
        public Guid ProductTypeId { get; set; }
        public Guid BrandId { get; set; }
        public string CountryCode { get; set; }
        public int CompanyIndex { get; set; }
        public bool HightlightProduct { get; set; }
        public string CompanyName { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
