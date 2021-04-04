using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingApp.Domain.Models
{
    public class ShippingPlanModel : AuditableEntityModel
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
        public string RefId
        {
            get
            {
                if (Product == null)
                {
                    return string.Empty;
                }

                return string.Join("-", SalesOrder, SalelineNumber, Product.ProductNumber);
            }
        }

        public int Id { get; set; }
        public string Prefix
        {
            get
            {
                return PrefixTable.ShippingPlan;
            }
        }

        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string SalesOrder { get; set; }
        public string SalelineNumber { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrder { get; set; }

        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }
        public int AccountNumber { get; set; }
        public int ProductLine { get; set; }

        [IgnoreMap]
        public ProductModel Product
        {
            get
            {
                if (ShippingPlanDetails == null || !ShippingPlanDetails.Any())
                {
                    return null;
                }

                return ShippingPlanDetails.ToArray()[0].Product;
            }
        }

        [IgnoreMap]
        public ShippingPlanDetailModel ShippingPlanDetail
        {
            get
            {
                if (ShippingPlanDetails == null || !ShippingPlanDetails.Any())
                {
                    return null;
                }

                return ShippingPlanDetails.ToArray()[0];
            }
        }

        public virtual ICollection<ShippingPlanDetailModel> ShippingPlanDetails { get; set; }
    }
}
