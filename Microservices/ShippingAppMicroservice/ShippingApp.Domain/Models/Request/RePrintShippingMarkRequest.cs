using System.ComponentModel.DataAnnotations;

namespace ShippingApp.Domain.Models
{
    public class RePrintShippingMarkRequest
    {
        [Required]
        public int ShippingMarkPrintingId { get; set; }
        [Required]
        public string RePrintedBy { get; set; }
    }
}

