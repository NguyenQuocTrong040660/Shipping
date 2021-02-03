using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Remarks { get; set; }
       
    }
}
