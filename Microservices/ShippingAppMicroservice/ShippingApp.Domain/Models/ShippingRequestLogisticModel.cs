
namespace ShippingApp.Domain.Models
{
    public class ShippingRequestLogisticModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public float GrossWeight { get; set; }
        public string CustomDeclarationNumber { get; set; }
        public string TrackingNumber { get; set; }

        public int TotalPackages { get; set; }

        public string Forwarder { get; set; }
        public float NetWeight { get; set; }
        public string Dimension { get; set; }

        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }

        public int ShippingRequestId { get; set; }
        public virtual ShippingRequestModel ShippingRequest { get; set; }
    }
}
