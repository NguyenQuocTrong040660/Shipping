using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ShippingRequestModel : AuditableEntityModel
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
                return PrefixTable.ShippingRequest;
            }
        }
        public string Notes { get; set; }
        public string Status { get; set; }
        
        public string AccountNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public string CustomerName { get; set; }
        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }

        public DateTime PickupDate { get; set; }

        public virtual ICollection<ShippingRequestLogisticModel> ShippingRequestLogistic { get; set; }
        public virtual ICollection<ShippingPlanModel> ShippingPlans { get; set; }
        public virtual ICollection<ShippingMarkModel> ShippingMarks { get; set; }
    }
}
