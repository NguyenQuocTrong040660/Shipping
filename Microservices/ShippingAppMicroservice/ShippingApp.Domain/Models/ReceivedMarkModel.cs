using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkModel : AuditableEntityModel
    {
        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        public int Id { get; set; }
        public string Prefix
        {
            get
            {
                return PrefixTable.ReceivedMark;
            }
        }
        public string Notes { get; set; }

        [IgnoreMap]
        public string WorkOrdersCollection { get; set; }

        public virtual ICollection<ReceivedMarkMovementModel> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrintingModel> ReceivedMarkPrintings { get; set; }
        public virtual ICollection<ReceivedMarkSummaryModel> ReceivedMarkSummaries { get; set; }
    }
}
