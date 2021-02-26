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
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesID { get; set; }
        public int SemlineNumber { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrder { get; set; }
        public virtual ICollection<ShippingRequestLogisticModel> ShippingRequestLogistics { get; set; }
        public virtual ICollection<ShippingRequestDetailModel> ShippingRequestDetails { get; set; }
        public virtual ICollection<ShippingMarkModel> ShippingMarks { get; set; }
    }
}
