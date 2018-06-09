using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class UserManager
    {
        private readonly UserManager<User> userManager;
        private readonly IBaseRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        public static IIdentity IdentityUser { get; private set; }


        public UserManager(UserManager<User> userManager, IBaseRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            IdentityUser = httpContextAccessor.HttpContext.User.Identity;
        }

        public User Load(string name)
        {
            return GetUserNavs().FirstOrDefault(a => a.UserName == name); 
        }

        public User CurrentUser()
        {
            return GetUserNavs().Where(u => u.UserName == IdentityUser.Name)
                .FirstOrDefault();
        }

        public Author CurrentAuthor()
        {
            var user = CurrentUser();
            return user != null && user.Author != null ? user.Author : null;
        }

        public async Task<ICollection<string>> CurrentUserRolesAsync()
        {
            var user = CurrentUser();
            if (user == null)
                throw new Exception("Текущий пользователь отсутствует");

            return await userManager.GetRolesAsync(user);
        }

        private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, Author> GetUserNavs()
        {
            return repository.Include<User, Author>(a => a.Author);
        }
    }
}
