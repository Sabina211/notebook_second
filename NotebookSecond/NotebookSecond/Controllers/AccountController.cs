using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotebookSecond.Entities;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginUser { ReturnUrl = returnUrl }) ;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = registerUser.Email,
                    UserName = registerUser.Login
                };

                var result = await userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "WorkersList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            /*else
            {
                ModelState.AddModelError("Login", "ошибки при валидации");
            }*/
            return View(registerUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(loginUser.Login, loginUser.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(loginUser.ReturnUrl) & Url.IsLocalUrl(loginUser.ReturnUrl))
                    {
                        return Redirect(loginUser.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "WorkersList");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректный логин и/или пароль");
                }
            }
           
            return View(loginUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "WorkersList");
        }
    }
}
