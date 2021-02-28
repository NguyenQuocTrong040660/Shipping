using ShippingApp.Domain.CommonEntities;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class MovementRequest : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<MovementRequestDetail> MovementRequestDetails { get; set; }
        public virtual ICollection<ReceivedMarkMovement> ReceivedMarkMovements { get; set; }
    }
}
