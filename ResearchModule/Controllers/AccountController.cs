﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;
using ResearchModule.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IBaseRepository manager;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager, IBaseRepository manager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.manager = manager;

        }

        public async Task<IActionResult> SaveUser(User user, Author author, string returnUrl)
        {
            var result = new Result();
            author.UserId = user.UserName;

            result.Set(author.Id == 0
                ? manager.Create(author).Error
                : manager.Update(author).Error);
            
            await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Redirect(returnUrl);

            return RedirectToAction("ChangeUser", user.UserName, returnUrl);
        }

        public IActionResult ChangeUser(string name, string returnUrl)
        {
            var currUser = User.Identity.Name;
            if (currUser == name || currUser == "admin0")
            {
                var user = manager.Include<User, Author>(u => u.Author)
                       .Where(u => u.UserName == name)
                       .FirstOrDefault();
                ViewData["returnUrl"] = returnUrl;
                return View(user);
            }
            ViewData["permissionError"] = string.Concat("Нет прав на редактирование пользователя ", name);
            return Redirect(returnUrl);
        }

        public IActionResult Profile(string name)
        {
            var user = manager.Include<User, Author>(u => u.Author)
                .Where(u => u.UserName == name)
                .FirstOrDefault();

            return View(user);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await userManager.FindByNameAsync(loginViewModel.UserName);

            if (await LoginUser(user, loginViewModel.Password))
            {
                if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                    return RedirectToAction("Publications", "Publication");
                return Redirect(loginViewModel.ReturnUrl);
            }


            ModelState.AddModelError("", "Название/пароль не найдены");
            return View(loginViewModel);
        }

        private async Task<bool> LoginUser(User user, string pass)
        {
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, pass, false, false);
                return result.Succeeded;
            }
            return false;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginViewModel.UserName);
                bool res = false;
                if (user == null)
                {
                    var result = await userManager
                        .CreateAsync(new User() { UserName = loginViewModel.UserName }, loginViewModel.Password);
                    res = result.Succeeded;
                }
                else
                {
                    res = await LoginUser(user, loginViewModel.Password);
                }
                /*res = (user == null)
                    ? (await userManager
                        .CreateAsync(new User() { UserName = loginViewModel.UserName }, loginViewModel.Password))
                        .Succeeded
                    : await LoginUser(user, loginViewModel.Password); */

                if (res)
                {
                    return RedirectToAction("Publications", "Publication");
                }

            }
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Base");
        }
    }
}