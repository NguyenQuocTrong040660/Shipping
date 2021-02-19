using AutoMapper;
using System;

namespace UserManagement.Application.Common.Results
{
    public class UserResult
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool LockoutEnabled { get; set; }
        [IgnoreMap]
        public string RoleName { get; set; }
        
    }
}
