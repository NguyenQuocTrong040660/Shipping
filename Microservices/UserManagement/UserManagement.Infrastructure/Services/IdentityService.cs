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
using UserManagement.Infrastructure.Extensions;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        
        public IdentityService(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstAsync(u => string.Equals(userName, u.UserName));
            return user.Id;
        }

        public async Task<(Result, string)> CreateUserAsync(string userName, 
            string password, 
            bool mustChangePassword = false, 
            string email = null)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email ?? userName,
                LockoutEnabled = false,
                LockoutEnd = DateTime.Now,
                RequireChangePassword = mustChangePassword
            };

            var result = await _userManager.CreateAsync(user, password);
            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> AssignUserToRole(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
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

        public async Task<(Result, string)> GenerateNewPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return (Result.Failure("Can not be found the user"), string.Empty);
            }

            var temporaryPassword = CreatePassword(10);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var setNewPasswordResult = await _userManager.ResetPasswordAsync(user, token, temporaryPassword);

            if (!setNewPasswordResult.Succeeded)
            {
                return (setNewPasswordResult.ToApplicationResult(), string.Empty);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return (Result.Success(), temporaryPassword);
        }

        public async Task<(Result, string)> CreateUserWithTemporaryPasswordAsync(string email, string userName, string roleId)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return (Result.Failure("User has been existed in system"), string.Empty);
            }

            var temporaryPassword = CreatePassword(10);

            (Result result, string userId) = await CreateUserAsync(userName, temporaryPassword, false, email);

            if (result.Succeeded)
            {
                user = await _userManager.FindByIdAsync(userId);

                var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id.Equals(roleId));

                if (role == null)
                {
                    throw new ArgumentNullException(nameof(role));
                }

                await AssignUserToRole(user, role.Name);
            }

            return (result, temporaryPassword);
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
            user.LockoutEnabled = false;
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

        public async Task<string> GetRoleUserAsync(string userName)
        {
            var roles = await GetRolesUserAsync(userName);
            return roles.FirstOrDefault();
        }
    }
}
