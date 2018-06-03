using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserManager(UserManager<User> userManager,
            SignInManager<User> signInManager, IBaseRepository repository, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repository = repository;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;

        }

        public User Load(string name)
        {
            return GetUserNavs().FirstOrDefault(a => a.UserName == name); 
        }

        public User CurrentUser()
        {
            return GetUserNavs().Where(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name)
                .FirstOrDefault();
        }

        public Author CurrentAuthor()
        {
            var user = CurrentUser();
            if (user != null && user.Author != null)
                return user.Author;

            return null;
        }

        public async Task<ICollection<string>> CurrentUserRolesAsync()
        {
            var user = CurrentUser();
            if (user != null)
                return await userManager.GetRolesAsync(user);
            return null;
        }

        private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, Author> GetUserNavs()
        {
            return repository.Include<User, Author>(a => a.Author);
        }
    }
}
