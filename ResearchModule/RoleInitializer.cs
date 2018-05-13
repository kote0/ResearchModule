using Microsoft.AspNetCore.Identity;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule
{
    public class RoleInitializer
    {
        public const string UserRole = "user";

        public const string AdminRole = "admin";

        public const string AnalystRole = "admin";


        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin0";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync(AdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }
            if (await roleManager.FindByNameAsync(UserRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = "admin@gmail.com", UserName = adminEmail };

                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    var roles = new List<string>();
                    roles.Add(UserRole);
                    roles.Add(AdminRole);
                    roles.Add(AnalystRole);

                    await userManager.AddToRolesAsync(admin, roles);
                }
            }
        }
    }
}
