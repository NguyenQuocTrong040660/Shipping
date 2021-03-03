using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMarkShipping : AuditableEntity
    {
        public int ShippingMarkId { get; set; }
        public int ProductId { get; set; }
        public int ShippingRequestId { get; set; }
        public int Quantity { get; set; }
        public virtual ShippingMark ShippingMark { get; set; }
        public virtual ShippingRequest ShippingRequest { get; set; }
        public virtual Product Product { get; set; }
    }
}
