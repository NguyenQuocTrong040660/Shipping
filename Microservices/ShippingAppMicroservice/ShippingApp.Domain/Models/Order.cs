using ShippingApp.Domain.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class Order
    {
        public Customer Customer { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
