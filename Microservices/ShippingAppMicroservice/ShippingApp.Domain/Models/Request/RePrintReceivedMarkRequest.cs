using System.ComponentModel.DataAnnotations;

namespace ShippingApp.Domain.Models
{
    public class RePrintReceivedMarkRequest
    {
        [Required]
        public int ReceivedMarkPrintingId { get; set; }
        [Required]
        public string RePrintedBy { get; set; }
    }
}

