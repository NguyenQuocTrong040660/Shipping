using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMark : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public int Sequence { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int PrintCount { get; set; }
        public int ProductId { get; set; }
        public int MovementRequestId { get; set; }


        public string LastPrePrintBy { get; set; }
        public DateTime? LastPrePrint { get; set; }
        public string ParentId { get; set; }

        public virtual MovementRequest MovementRequest { get; set; }
        public virtual Product Product { get; set; }
    }
}
