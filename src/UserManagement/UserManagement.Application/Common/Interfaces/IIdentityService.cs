using System.Threading.Tasks;
using UserManagement.Application.Common.Results;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using UserManagement.Domain.Common;

namespace UserManagement.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<string> GetUserIdAsync(string userName);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<SignInResult> SignInAsync(string userName, string password, bool rememberMe);

        Task<IList<string>> GetRolesUserAsync(string userName);

        Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<Result> GenerateNewPasswordAsync(string email);

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<List<ApplicationUser>> GetUsersAsync();

        Task<IdentityResult> AssignUserToRole(ApplicationUser user, string role);

        Task ResetAccessFailedCountAsync(string userId);

        Task LockUserAsync(string userId);

        Task UnlockUserAsync(string userId);
    }
}
