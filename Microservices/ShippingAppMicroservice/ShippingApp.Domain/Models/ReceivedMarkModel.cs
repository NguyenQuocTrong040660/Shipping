using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ReceivedMarkModel : AuditableEntityModel
    {
        public int Id { get; set; }
        public string Prefix
        {
            get
            {
                return PrefixTable.ReceivedMark;
            }
        }
        public string Notes { get; set; }

        public virtual ICollection<ReceivedMarkMovementModel> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrintingModel> ReceivedMarkPrintings { get; set; }

        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        [IgnoreMap]
        public string WorkOrdersMovementCollection { get; set; }
    }
}
