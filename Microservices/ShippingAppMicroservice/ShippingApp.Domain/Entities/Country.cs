using ShippingApp.Domain.CommonEntities;
using System;
namespace ShippingApp.Domain.Entities
{
    public class Country : AuditableEntity
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
