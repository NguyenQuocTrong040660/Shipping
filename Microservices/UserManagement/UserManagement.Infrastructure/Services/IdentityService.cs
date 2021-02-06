using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;
using UserManagement.Domain.Common;
using UserManagement.Domain.Enums;
using UserManagement.Infrastructure.Extensions;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        
        public IdentityService(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            return user.UserName;
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstAsync(u => string.Equals(userName, u.UserName));
            return user.Id;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, bool mustChangePassword = false)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                LockoutEnabled = true,
                RequireChangePassword = mustChangePassword
            };

            var result = await _userManager.CreateAsync(user, password);

            await AssignUserToRole(user, Roles.ITAdministrator);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> AssignUserToRole(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<SignInResult> SignInAsync(string userName, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure: false);
        }

        public async Task<IList<string>> GetRolesUserAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }

        public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("Can not be found the user");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changePasswordResult.Succeeded)
            {
                return changePasswordResult.ToApplicationResult();
            }

            await _signInManager.RefreshSignInAsync(user);

            return Result.Success();
        }

        public async Task<Result> GenerateNewPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure("Can not be found the user");
            }

            var temporaryPassword = CreatePassword(10);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var setNewPasswordResult = await _userManager.ResetPasswordAsync(user, token, temporaryPassword);

            if (!setNewPasswordResult.Succeeded)
            {
                return setNewPasswordResult.ToApplicationResult();
            }

            user.RequireChangePassword = true;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return Result.Success();
        }

        public async Task<Result> CreateUserWithTemporaryPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Result.Failure("User has been existed in system");
            }

            var temporaryPassword = CreatePassword(10);
            (Result result, string userId) = await CreateUserAsync(email, temporaryPassword, true);

            if (result.Succeeded)
            {
                user = await _userManager.FindByIdAsync(userId);
                await AssignUserToRole(user, Roles.Employee);
            }

            return result;
        }

        public async Task<ApplicationUser> GetUserByIdentifierAsync(string identifier)
        {
            var user = await _userManager.FindByIdAsync(identifier);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(identifier);

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(identifier);
                }
            }

            return user;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task LockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddDays(36500);

            await _userManager.UpdateAsync(user);
        }

        public async Task UnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.LockoutEnd = DateTime.Now;
            await _userManager.UpdateAsync(user);
        }

        public async Task ResetAccessFailedCountAsync(string userId)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = userId
            };
            await _userManager.ResetAccessFailedCountAsync(user);
        }

        internal string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
