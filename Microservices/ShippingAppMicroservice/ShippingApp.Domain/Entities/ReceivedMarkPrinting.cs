using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMarkPrinting : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public int Sequence { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int PrintCount { get; set; }
        public int ParentId { get; set; }

        public string Revision { get; set; }

        public string RePrintingBy { get; set; }
        public DateTime? RePrintingDate { get; set; }

        public string PrintingBy { get; set; }
        public DateTime? PrintingDate { get; set; }

        public int ProductId { get; set; }
        public int ReceivedMarkId { get; set; }
        public int? ShippingMarkId { get; set; }

        public virtual ReceivedMark ReceivedMark { get; set; }
        public virtual Product Product { get; set; }
        public virtual ShippingMark ShippingMark { get; set; }
    }
}
