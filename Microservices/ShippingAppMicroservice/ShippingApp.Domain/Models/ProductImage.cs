using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class ProductImage
    {
        public string ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public string ImageUrl{ get; set; }
        public string ImageBase64Encrypt { get; set; }
    }
}
