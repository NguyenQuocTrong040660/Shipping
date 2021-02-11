using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class Config : AuditableEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
