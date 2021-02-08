using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UserManagement.Domain.Common
{
    public class ApplicationUser : IdentityUser
    {
        public bool RequireChangePassword { get; set; }
    }
}
