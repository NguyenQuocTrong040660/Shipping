using System.ComponentModel.DataAnnotations;

namespace ShippingApp.Domain.Models
{
    public class PrintShippingMarkRequest
    {
        [Required]
        public int ShippingMarkId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string PrintedBy { get; set; }
    }
}

