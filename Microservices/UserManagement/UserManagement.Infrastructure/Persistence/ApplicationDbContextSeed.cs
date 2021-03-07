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

        public static async Task SeedUserTestingAsync(UserManager<ApplicationUser> userManager)
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "user01",
                    Email = "user01@gmail.com",
                },
                new ApplicationUser
                {
                    UserName = "user02",
                    Email = "user02@gmail.com",
                },
                new ApplicationUser
                {
                    UserName = "user03",
                    Email = "user03@gmail.com",
                },
                new ApplicationUser
                {
                    UserName = "user04",
                    Email = "user04@gmail.com",
                },
                new ApplicationUser
                {
                    UserName = "user05",
                    Email = "user05@gmail.com",
                },
                new ApplicationUser
                {
                    UserName = "user06",
                    Email = "user06@gmail.com",
                }
            };

            var roles = new List<string>
            {
                Roles.ITAdministrator,// userName: user01
                Roles.ShippingDept,   // userName: user02
                Roles.Manager,        // userName: user03
                Roles.LogisticsDept,  // userName: user04
                Roles.PlanningDept,   // userName: user05
                Roles.FAQDept,        // userName: user06
            };

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];

                if (userManager.Users.All(u => u.UserName != user.UserName))
                {
                    user.LockoutEnabled = false;
                    user.LockoutEnd = DateTime.Now;

                    await userManager.CreateAsync(user, "userTesting@123456");
                    await userManager.AddToRoleAsync(user, roles[i]);
                }
            }
        }
    }
}