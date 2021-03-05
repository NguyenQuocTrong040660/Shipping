using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ProductModel : AuditableEntityModel
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
                return PrefixTable.Product;
            }
        }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Notes { get; set; }
        public string QtyPerPackage { get; set; }

        public virtual ICollection<ShippingPlanDetailModel> ShippingPlanDetails { get; set; }
        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }
        public virtual ICollection<ShippingRequestDetailModel> ShippingRequestDetails { get; set; }
        public virtual ICollection<WorkOrderDetailModel> WorkOrderDetails { get; set; }

        public virtual ICollection<ReceivedMarkMovementModel> ReceivedMarkMovements { get; set; }
        public virtual ICollection<ReceivedMarkPrintingModel> ReceivedMarkPrintings { get; set; }
        public virtual ICollection<ReceivedMarkSummaryModel> ReceivedMarkSummaries { get; set; }

        public virtual ICollection<ShippingMarkPrintingModel> ShippingMarkPrintings { get; set; }
        public virtual ICollection<ShippingMarkShippingModel> ShippingMarkShippings { get; set; }
        public virtual ICollection<ShippingMarkSummaryModel> ShippingMarkSummaries { get; set; }
    }
}
