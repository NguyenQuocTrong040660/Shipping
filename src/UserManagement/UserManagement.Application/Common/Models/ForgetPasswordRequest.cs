using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Common.Models
{
    public class ForgetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
