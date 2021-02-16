namespace ShippingApp.Domain.Models
{
    public class WorkOrderDetailModel : AuditableEntityModel
    {
        public int Id { get; set; }

        public int WorkOrderId { get; set; }
        public virtual WorkOrderModel WorkOrder { get; set; }

        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
