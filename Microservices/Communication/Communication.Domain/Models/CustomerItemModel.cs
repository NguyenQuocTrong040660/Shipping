using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Domain.Models
{
    public class CustomerItemModel
    {
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string HomeAddress { get; set; }
        public string Remark { get; set; }
    }
}
