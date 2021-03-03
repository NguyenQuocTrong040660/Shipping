using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class ShippingMarkModel : AuditableEntityModel
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
                return PrefixTable.ShippingMark;
            }
        }
        
        public string Notes { get; set; }

        public virtual ICollection<ShippingMarkPrintingModel> ShippingMarkPrintings { get; set; }
        public virtual ICollection<ShippingMarkShippingModel> ShippingMarkShippings { get; set; }
        public virtual ICollection<ShippingMarkSummaryModel> ShippingMarkSummaries { get; set; }
    }
}
