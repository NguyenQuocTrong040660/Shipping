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

        public virtual ICollection<ShippingRequestLogisticModel> ShippingRequestLogistic { get; set; }
        public virtual ICollection<ShippingRequestDetailModel> ShippingRequestDetails { get; set; }
        public virtual ICollection<ShippingMarkModel> ShippingMarks { get; set; }
    }
}
