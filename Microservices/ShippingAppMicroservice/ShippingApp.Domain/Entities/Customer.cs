using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Note { get; set; }
    }
}
