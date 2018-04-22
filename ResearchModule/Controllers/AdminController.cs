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

namespace ResearchModule.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IBaseRepository repository;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(UserManager<User> userManager,
            SignInManager<User> signInManager, IBaseRepository repository, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repository = repository;
            this.roleManager = roleManager;

        }

        public PartialViewResult LoadUsers(int first = 1)
        {
            var list = repository.Page<User>(first);
            var pageInfo = new PageInfo(first, list.Count());
            pageInfo.SetUrl("LoadUsers", "Admin");
            ViewData["pageInfo"] = pageInfo;

            return PartialView(list);
        }

        public PartialViewResult LoadRoles(int first = 1)
        {
            var list = repository.Page<IdentityRole>(first);
            var pageInfo = new PageInfo(first, list.Count());
            pageInfo.SetUrl("LoadRoles", "Admin");
            ViewData["pageInfo"] = pageInfo;

            return PartialView(list);
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}