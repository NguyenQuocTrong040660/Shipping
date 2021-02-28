    using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMarkSummary : AuditableEntity
    {
        public int ReceivedMarkId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalPackage { get; set; }

        public virtual ReceivedMark ReceivedMark { get; set; }
        public virtual Product Product { get; set; }
    }
}
