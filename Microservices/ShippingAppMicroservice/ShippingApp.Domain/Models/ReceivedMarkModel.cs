using System;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int PrintCount { get; set; }
        public int ProductId { get; set; }
        public int MovementRequestId { get; set; }
        public virtual MovementRequestModel MovementRequest { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
