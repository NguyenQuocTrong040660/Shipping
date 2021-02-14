using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class MovementRequestModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<WorkOrderModel> WorkOrders { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
