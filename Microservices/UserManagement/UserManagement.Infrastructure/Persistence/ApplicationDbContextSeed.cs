using Microsoft.AspNetCore.Identity;
using System;
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
            var defaultUser = new ApplicationUser { UserName = "admin", Email = "admin@gmail.com" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                defaultUser.LockoutEnabled = false;
                defaultUser.LockoutEnd = DateTime.Now;
                await userManager.CreateAsync(defaultUser, "ad@123456");
            }

            var user = await userManager.FindByNameAsync("admin");

            if (user != null)
            {
                await userManager.AddToRoleAsync(user, Roles.SystemAdministrator);
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
                        Name = Roles.ITAdministrator
                    },
                    new IdentityRole
                    {
                        Name = Roles.Manager
                    },
                    new IdentityRole
                    {
                        Name = Roles.ShippingDept
                    },
                    new IdentityRole
                    {
                        Name = Roles.SystemAdministrator
                    },
                    new IdentityRole
                    {
                        Name = Roles.FAQDept
                    },
                    new IdentityRole
                    {
                        Name = Roles.PlanningDept
                    },
                    new IdentityRole
                    {
                        Name = Roles.LogisticsDept
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
