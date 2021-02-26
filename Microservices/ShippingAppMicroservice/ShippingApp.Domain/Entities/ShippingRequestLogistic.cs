using ShippingApp.Domain.CommonEntities;

namespace ShippingApp.Domain.Entities
{
    public class ShippingRequestLogistic : AuditableEntity
    {
        public int Id { get; set; }
        public float GrossWeight { get; set; }
        public string BillToCustomer { get; set; }
        public string ReceiverCustomer { get; set; }
        public string ReceiverAddress { get; set; }
        public string CustomDeclarationNumber { get; set; }
        public string TrackingNumber { get; set; }
        public int ShippingRequestId { get; set; }
        public string Notes { get; set; }
        public virtual ShippingRequest ShippingRequest { get; set; }
    }
}
