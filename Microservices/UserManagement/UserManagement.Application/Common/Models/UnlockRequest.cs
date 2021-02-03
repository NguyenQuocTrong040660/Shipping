using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Common.Models
{
    public class UnlockRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
