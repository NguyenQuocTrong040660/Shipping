using Microsoft.AspNetCore.Identity;

namespace UserManagement.Domain.Common
{
    public class ApplicationUser : IdentityUser
    {
        public bool RequireChangePassword { get; set; }
    }
}
