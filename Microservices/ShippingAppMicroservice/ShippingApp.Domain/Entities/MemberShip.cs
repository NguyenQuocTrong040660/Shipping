using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class MemberShip
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
