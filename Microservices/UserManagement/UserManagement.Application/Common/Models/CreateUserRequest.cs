namespace UserManagement.Application.Common.Models
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
    }
}
