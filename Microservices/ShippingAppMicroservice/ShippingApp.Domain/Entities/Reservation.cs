using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime DateSet { get; set; }
        public TimeSpan TimeSet { get; set; }
        public int CountPerson { get; set; }
        public string ServiceName { get; set; }
    }
}
