using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMarkReceived : AuditableEntity
    {
        public int ShippingMarkId { get; set; }
        public int ReceivedMarkId { get; set; }
        public virtual ShippingMark ShippingMark { get; set; }
        public virtual ReceivedMark ReceivedMark { get; set; }
    }
}
