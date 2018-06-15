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

        public const string AnalystRole = "analyst";

        public const string AdminName = "admin0";


        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync(AdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }
            if (await roleManager.FindByNameAsync(UserRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole));
            }
            if (await roleManager.FindByNameAsync(AnalystRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(AnalystRole));
            }
            
            if (await userManager.FindByNameAsync(AdminName) == null)
            {
                User admin = new User { Email = "admin@gmail.com", UserName = AdminName };

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
