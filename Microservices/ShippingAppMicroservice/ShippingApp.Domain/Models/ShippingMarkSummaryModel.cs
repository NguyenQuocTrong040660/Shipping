
namespace ShippingApp.Domain.Models
{
    public class ShippingMarkSummaryModel : AuditableEntityModel
    {
        public int ShippingMarkId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalPackage { get; set; }
        public virtual ShippingMarkModel ShippingMark { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
