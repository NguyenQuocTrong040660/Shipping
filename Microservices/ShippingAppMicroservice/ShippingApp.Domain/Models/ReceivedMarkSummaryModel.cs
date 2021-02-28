
namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkSummaryModel : AuditableEntityModel
    {
        public int ReceivedMarkId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalPackage { get; set; }
        public virtual ReceivedMarkModel ReceivedMark { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
