using System;

namespace ShippingApp.Domain.Models
{
    public class ProductTypeModel
    {
        public int Id { get; set; }
        public string ProductTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
