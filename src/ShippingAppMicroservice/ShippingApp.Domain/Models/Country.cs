using System;
using System.Collections.Generic;
using System.Text;
namespace ShippingApp.Domain.Models
{
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
