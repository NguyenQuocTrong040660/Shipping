using ShippingApp.Domain.CommonEntities;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMark : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<ShippingMarkPrinting> ShippingMarkPrintings { get; set; }
        public virtual ICollection<ShippingMarkShipping> ShippingMarkShippings { get; set; }
        public virtual ICollection<ShippingMarkSummary> ShippingMarkSummaries { get; set; }
    }
}
