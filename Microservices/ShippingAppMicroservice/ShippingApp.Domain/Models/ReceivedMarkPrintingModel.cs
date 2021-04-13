using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkPrintingModel : AuditableEntityModel
    {
        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        [IgnoreMap]
        public WorkOrderModel WorkOrder { get; set; }


        public int Id { get; set; }
        public string Prefix
        {
            get
            {
                return PrefixTable.ReceivedMarkPrinting;
            }
        }
        public int Sequence { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int PrintCount { get; set; }
        public string ParentId { get; set; }

        public string Revision { get; set; }

        public string RePrintingBy { get; set; }
        public DateTime? RePrintingDate { get; set; }

        public string PrintingBy { get; set; }
        public DateTime? PrintingDate { get; set; }

        public int ProductId { get; set; }
        public int ReceivedMarkId { get; set; }
        public int? ShippingMarkId { get; set; }

        public virtual ReceivedMarkModel ReceivedMark { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual ShippingMarkModel ShippingMark { get; set; }

        public int MovementRequestId { get; set; }
        public virtual MovementRequestModel MovementRequest { get; set; }
    }
}
