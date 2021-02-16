using System;

namespace ShippingApp.Domain.Models
{
    public class CountryModel : AuditableEntityModel
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
