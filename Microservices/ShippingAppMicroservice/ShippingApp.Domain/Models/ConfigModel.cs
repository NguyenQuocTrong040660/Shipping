using System;

namespace ShippingApp.Domain.Models
{
    public class ConfigModel : AuditableEntityModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
