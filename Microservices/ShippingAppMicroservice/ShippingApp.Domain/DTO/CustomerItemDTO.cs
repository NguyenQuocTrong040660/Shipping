using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.DTO
{
    public class CustomerItemDTO
    {
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Birththday { get; set; }
        public string DateSet { get; set; }
        public int CountPerson { get; set; }
        public string Service { get; set; }
    }
}
