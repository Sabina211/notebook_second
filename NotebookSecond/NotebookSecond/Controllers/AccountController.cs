using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<AccountController> logger;
        private HttpClient httpClient { get; set; }
        //ApiAccountData api = new ApiAccountData();

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, HttpClient httpClient)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginUser { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                string url = @"https://localhost:5005/api/Account/register";
                var content = new StringContent(JsonConvert.SerializeObject(registerUser), Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync(url, content).Result;
                bool success = CheckResult(result);
                if (success)
                {
                    //await signInManager.SignInAsync(user, false);
                    logger.LogInformation("Зарегистрировался новый пользователь с логином {0}", registerUser.Login);
                    return RedirectToAction("index", "Worker");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Не удалось зарегистрировать пользоваткеля");
                    /*foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }*/
                }
            }
            return View(registerUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                string url = @"https://localhost:5005/api/Account/login";
                var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
                var bool2 = Request.Cookies.Any();
                var result = httpClient.PostAsync(url, content).Result;
                //var cookies = result.Headers.GetValues(HeaderNames.SetCookie);
                bool success = CheckResult(result);
                if (success)
                {
                    if (!string.IsNullOrEmpty(loginUser.ReturnUrl) & Url.IsLocalUrl(loginUser.ReturnUrl))
                    {
                        return Redirect(loginUser.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "Worker");
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
            string url = @"https://localhost:5005/api/Account/logout";
            var result = httpClient.PostAsync(url, null).Result;
            return RedirectToAction("index", "Worker");
        }

        private bool CheckResult(HttpResponseMessage result)
        {
            if (!(result.StatusCode.ToString() == "OK"))
            {
                logger.LogError($"Ошибка при обращении к апи result.StatusCode = {result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}");
                return false;
            }
            else return true;
        }
    }
}
