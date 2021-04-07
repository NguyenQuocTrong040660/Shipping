using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;

namespace ShippingApp.Domain.Models
{
    public class ShippingMarkPrintingModel : AuditableEntityModel
    {
        [IgnoreMap]
        public string Identifier
        {
            get
            {
                return string.Concat(Prefix, Id);
            }
        }

        [IgnoreMap]
        public ShippingMarkShippingModel ShippingMarkShipping { get; set; }

        public int Id { get; set; }
        public string Prefix
        {
            get
            {
                return PrefixTable.ShippingMarkPrinting;
            }
        }

        public string Revision { get; set; }
        public int Sequence { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int PrintCount { get; set; }

        public string RePrintingBy { get; set; }
        public DateTime? RePrintingDate { get; set; }

        public string PrintingBy { get; set; }
        public DateTime? PrintingDate { get; set; }

        public int ProductId { get; set; }
        public int ShippingMarkId { get; set; }

        public virtual ShippingMarkModel ShippingMark { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
