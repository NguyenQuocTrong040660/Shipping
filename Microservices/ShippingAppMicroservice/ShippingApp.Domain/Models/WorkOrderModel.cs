using System;

namespace ShippingApp.Domain.Models
{
    public class WorkOrderModel
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public int Quantity { get; set; }
        public int MovingQuantity { get; set; }
        public int RemainQuantity { get; set; }
        public string Notes { get; set; }

        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }

        public int MovementRequestId { get; set; }
        public virtual MovementRequestModel MovementRequest { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
