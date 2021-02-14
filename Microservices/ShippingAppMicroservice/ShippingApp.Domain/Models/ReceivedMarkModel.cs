using System;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public int WorkOrderId { get; set; }
        public virtual WorkOrderModel WorkOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
