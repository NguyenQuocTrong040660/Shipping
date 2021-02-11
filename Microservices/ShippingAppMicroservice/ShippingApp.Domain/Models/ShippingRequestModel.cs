using System;

namespace ShippingApp.Domain.Models
{
    public class ShippingRequestModel
    {
        public int Id { get; set; }
        public int SemlineNumber { get; set; }
        public string PurchaseOrder { get; set; }
        public int QuantityOrder { get; set; }
        public float SalesPrice { get; set; }
        public string CustomerName { get; set; }
        public string SalesID { get; set; }
        public string ShippingMode { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
