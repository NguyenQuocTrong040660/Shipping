namespace ShippingApp.Domain.Models
{
    public class ShippingMarkReceivedModel : AuditableEntityModel
    {
        public int ShippingMarkId { get; set; }
        public int ShippingRequestId { get; set; }
        public virtual ShippingMarkModel ShippingMark { get; set; }
        public virtual ShippingRequestModel ShippingRequest { get; set; }
    }
}
