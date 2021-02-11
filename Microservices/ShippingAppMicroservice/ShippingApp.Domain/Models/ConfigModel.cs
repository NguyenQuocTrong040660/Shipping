using System;

namespace ShippingApp.Domain.Models
{
    public class ConfigModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
