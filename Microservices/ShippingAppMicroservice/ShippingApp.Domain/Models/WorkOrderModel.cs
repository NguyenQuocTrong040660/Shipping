using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System.Collections.Generic;

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
        public virtual ICollection<WorkOrderDetailModel> WorkOrderDetails { get; set; }
        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }
    }
}
