using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Models;
using ResearchModule.Components.Models;
using ResearchModule.Components.Page;

namespace ResearchModule.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        //private readonly UserManager<User> userManager;
        //private readonly SignInManager<User> signInManager;
        private readonly IBaseRepository repository;
        //private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserPage userPage;

        public AdminController(UserManager<User> userManager,
            SignInManager<User> signInManager, IBaseRepository repository, 
            RoleManager<IdentityRole> roleManager)
        {
            //this.userManager = userManager;
            //this.signInManager = signInManager;
            // this.roleManager = roleManager;
            this.repository = repository;
           
            this.userPage = new UserPage(repository);

        }

        public PartialViewResult LoadUsers(int first = 1)
        {
            return PartialView(userPage.CreatePagination(first, "LoadUsers", "Admin"));
        }

        public PartialViewResult LoadRoles(int first = 1)
        {
            var list = repository.Page<IdentityRole>(first);
            var count = repository.LongCount<IdentityRole>();
            var pageInfo = new PageInfo(first, count);
            pageInfo.SetUrl("LoadRoles", "Admin");
            ViewData["pageInfo"] = pageInfo;

            return PartialView(list);
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> GiveRole(string name)
        //{
        //    await userManager.FindByNameAsync(name);
        //    return View();
        //}
    }
}