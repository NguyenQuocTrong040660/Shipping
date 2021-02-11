using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ProductType: AuditableEntity
    {
        public int Id { get; set; }
        public string ProductTypeName { get; set; }
    }
}
