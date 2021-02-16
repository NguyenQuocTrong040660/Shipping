using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class MovementRequestModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }
        public virtual ICollection<ReceivedMarkModel> ReceivedMarks { get; set; }
    }
}
