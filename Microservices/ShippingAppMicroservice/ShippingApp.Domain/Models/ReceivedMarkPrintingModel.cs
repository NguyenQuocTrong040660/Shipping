using System;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkPrintingModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int PrintCount { get; set; }
        public string ParentId { get; set; }

        public string RePrintingBy { get; set; }
        public DateTime? RePrintingDate { get; set; }

        public string PrintingBy { get; set; }
        public DateTime? PrintingDate { get; set; }

        public int ProductId { get; set; }
        public int ReceivedMarkId { get; set; }

        public virtual ReceivedMarkModel ReceivedMark { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
