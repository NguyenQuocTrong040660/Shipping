using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Common.Models
{
    public class LockRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
