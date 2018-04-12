using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResearchModule.Models;
using ResearchModule.Repository.Interfaces;
using ResearchModule.ViewModels;
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
                        .CreateAsync(new User() { UserName = loginViewModel.UserName}, loginViewModel.Password);
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
