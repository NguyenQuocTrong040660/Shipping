using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingApp.Domain.Models
{
    public class WorkOrderModel : AuditableEntityModel
    {
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
        public string PartRevision { get; set; }
        public string ProcessRevision { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }

        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }

        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        [IgnoreMap]
        public string ProductName
        {
            get
            {
                if (Product == null)
                {
                    return string.Empty;
                }

                return Product.ProductName;
            }
        }

        [IgnoreMap]
        public string ProductNumber
        {
            get
            {
                if (Product == null)
                {
                    return string.Empty;
                }

                return Product.ProductNumber;
            }
        }

        [IgnoreMap]
        public int RemainQuantity
        {
            get
            {
                return Quantity - ReceviedMarkQuantity;
            }
        }

        [IgnoreMap]
        public int ReceviedMarkQuantity { get; set; }

        [IgnoreMap]
        public int MomentQuantity
        {
            get
            {
                if (MovementRequestDetails == null || !MovementRequestDetails.Any())
                {
                    return 0;
                }

                return MovementRequestDetails.Sum(x => x.Quantity);
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

        [IgnoreMap]
        public List<ReceivedMarkMovementModel> ReceivedMarkMovements { get; set; }
    }
}
