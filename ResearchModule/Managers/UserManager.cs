using Microsoft.AspNetCore.Identity;
using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;
using ResearchModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class UserManager
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IBaseRepository repository;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManager(UserManager<User> userManager,
            SignInManager<User> signInManager, IBaseRepository repository, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repository = repository;
            this.roleManager = roleManager;

        }

        public User Load(string name)
        {
            return repository.Include<User, Author>(a => a.Author)
                .FirstOrDefault(a => a.UserName == name); 
        }
    }
}
