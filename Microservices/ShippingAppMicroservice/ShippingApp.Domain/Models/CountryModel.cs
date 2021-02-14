using System;

namespace ShippingApp.Domain.Models
{
    public class CountryModel
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
