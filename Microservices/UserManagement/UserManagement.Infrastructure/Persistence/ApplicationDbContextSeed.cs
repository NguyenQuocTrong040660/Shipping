using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Common;
using UserManagement.Domain.Enums;

namespace UserManagement.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "admin@gmail.com", Email = "admin@gmail.com" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "ad@123456");
            }

            var user = await userManager.FindByNameAsync("admin@gmail.com");

            if (user != null)
            {
                if (!(await userManager.IsInRoleAsync(user, Roles.Administrator)))
                {
                    await userManager.AddToRoleAsync(user, Roles.Administrator);
                }
            }
        }

        public static async Task SeedDefaultRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole
                    {
                        Name = Roles.Administrator
                    },
                    new IdentityRole
                    {
                        Name = Roles.Customer
                    }
                };

                foreach (var item in roles)
                {
                    await roleManager.CreateAsync(item);
                }
            }
        }
    }
}
