using System.ComponentModel.DataAnnotations;

namespace ShippingApp.Domain.Models
{
    public class PrintReceivedMarkRequest
    {
        [Required]
        public int ReceivedMarkId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string PrintedBy { get; set; }
    }
}

