using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMarkSummary : AuditableEntity
    {
        public int ShippingMarkId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalPackage { get; set; }

        public virtual ShippingMark ShippingMark { get; set; }
        public virtual Product Product { get; set; }
    }
}
