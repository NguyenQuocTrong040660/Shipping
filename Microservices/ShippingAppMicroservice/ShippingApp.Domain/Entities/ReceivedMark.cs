using ShippingApp.Domain.CommonEntities;
using System.Collections.Generic;

namespace ShippingApp.Domain.Entities
{
    public class ReceivedMark : AuditableEntity
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<ReceivedMarkMovement> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrinting> ReceivedMarkPrintings { get; set; }
        public virtual ICollection<ReceivedMarkSummary> ReceivedMarkSummaries { get; set; }
        public virtual ICollection<ShippingMarkReceived> ShippingMarkReceiveds { get; set; }
    }
}
