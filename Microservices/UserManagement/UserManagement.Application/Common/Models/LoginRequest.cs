using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Common.Models
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
