using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Common.Models
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string RoleId { get; set; }
    }
}
