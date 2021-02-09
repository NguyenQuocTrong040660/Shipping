using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
namespace ShippingApp.Domain.Entities
{
    public class Product: AuditableEntity
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public int QtyPerPackage { get; set; }
    }
}
