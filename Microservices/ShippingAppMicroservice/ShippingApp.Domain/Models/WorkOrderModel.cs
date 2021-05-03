using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingApp.Domain.Models
{
    public class WorkOrderModel : AuditableEntityModel
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
                return PrefixTable.WorkOrder;
            }
        }
        public string RefId { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        [IgnoreMap]
        public ProductModel Product
        {
            get
            {
                if (WorkOrderDetails == null || !WorkOrderDetails.Any())
                {
                    return null;
                }

                return WorkOrderDetails.ToArray()[0].Product;
            }
        }

        [IgnoreMap]
        public WorkOrderDetailModel WorkOrderDetail
        {
            get
            {
                if (WorkOrderDetails == null || !WorkOrderDetails.Any())
                {
                    return null;
                }

                return WorkOrderDetails.ToArray()[0];
            }
        }

        [IgnoreMap]
        public int RemainQuantity
        {
            get
            {
                if (WorkOrderDetail == null || MovementRequestDetails == null)
                {
                    return 0;
                }

                return WorkOrderDetail.Quantity - MovementRequestDetails.Sum(x => x.Quantity);
            }
        }

        [IgnoreMap]
        public bool CanSelected
        {
            get
            {
                if (Status == null)
                {
                    return true;
                }

                return Status.Equals(nameof(WorkOrderStatus.Start));
            }
        }

        public string PartRevision { get; set; }
        public string ProcessRevision { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<WorkOrderDetailModel> WorkOrderDetails { get; set; }
        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }
    }
}
