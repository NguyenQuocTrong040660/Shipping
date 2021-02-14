using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public string QtyPerPackage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<WorkOrderModel> WorkOrders { get; set; }
        public virtual ICollection<ShippingMarkModel> ShippingMarks { get; set; }
        public virtual ICollection<ShippingRequestModel> ShippingRequests { get; set; }
    }
}
