using Microsoft.AspNetCore.Identity;
using System;

namespace UserManagement.Domain.Common
{
    public class ApplicationUser : IdentityUser
    {
        public bool RequireChangePassword { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
