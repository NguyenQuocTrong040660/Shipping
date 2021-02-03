using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class ProductOverview: AuditableEntity
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string HighLevelDesc { get; set; }
        public string MediumLevelDesc { get; set; }
        public string NormalLevelDesc { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public Guid ProductTypeId { get; set; }
        public Guid BrandId { get; set; }
        public string CountryCode { get; set; }
        public int CompanyIndex { get; set; }
        public bool HightlightProduct { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual Country Country { get; set; }
        public virtual Brand Brand { get; set; }
       
    }
}
