using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingApp.Domain.Models
{
    public class ShippingPlanModel : AuditableEntityModel
    {
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

        public string AccountNumber { get; set; }
        public int ProductLine { get; set; }

        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ShippingMode { get; set; }
        public float Amount { get; set; }

        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }

        public int? ShippingRequestId { get; set; }
        public virtual ShippingRequestModel ShippingRequest { get; set; }

        [IgnoreMap]
        public string Status
        {
            get
            {
                return ShippingRequestId.HasValue ? "Close" : "Start";
            }
        }

        [IgnoreMap]
        public bool CanSelected
        {
            get
            {
                return ShippingRequestId.HasValue;
            }
        }

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
    }
}
