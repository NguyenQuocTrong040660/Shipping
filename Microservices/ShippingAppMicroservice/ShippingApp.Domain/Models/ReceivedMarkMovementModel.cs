using AutoMapper;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkMovementModel : AuditableEntityModel
    {
        public int ReceivedMarkId { get; set; }
        public int ProductId { get; set; }
        public int MovementRequestId { get; set; }
        public int Quantity { get; set; }
        public int WorkOrderId { get; set; }

        public virtual ReceivedMarkModel ReceivedMark { get; set; }
        public virtual MovementRequestModel MovementRequest { get; set; }
        public virtual ProductModel Product { get; set; }

        [IgnoreMap]
        public string WorkOrderMomentRequest { get; set; }
        [IgnoreMap]
        public int TotalPackage { get; set; }
        [IgnoreMap]
        public int TotalQuantityPrinted { get; set; }
    }
}
