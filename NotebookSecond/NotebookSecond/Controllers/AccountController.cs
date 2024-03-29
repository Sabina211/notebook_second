﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotebookSecond.Data;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public AccountController(ILogger<AccountController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("httpClient");
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
            if (!ModelState.IsValid)
                return View(registerUser);
            var url = _httpClient.BaseAddress + "Accounts/register";
            var content = new StringContent(JsonConvert.SerializeObject(registerUser), Encoding.UTF8, "application/json");
            var result = _httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (success)
            {
                await LoginInSystem(registerUser.Login, result);
                _logger.LogInformation("Зарегистрировался новый пользователь с логином {0}", registerUser.Login);
                return RedirectToAction("index", "Worker");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Не удалось зарегистрировать пользователя\n {result.Content.ReadAsStringAsync().Result.UnescapeUnicode()}");
            }
            return View(registerUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            if (!ModelState.IsValid)
                return View(loginUser);

                var url = _httpClient.BaseAddress + "Accounts/login";
                var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
                var result = _httpClient.PostAsync(url, content).Result;
                bool success = CheckResult(result);

                if (success)
                {
                    await LoginInSystem(loginUser.Login, result);
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
            return View(loginUser);
        }

        private async Task LoginInSystem(string login, HttpResponseMessage result)
        {
            UserWithRolesEdit currentUser = JsonConvert.DeserializeObject<UserWithRolesEdit>(result.Content.ReadAsStringAsync().Result);

            var claims = (currentUser.UserRoles.Count != 0) ?
                 new List<Claim> { new Claim(ClaimTypes.Name, login), new Claim(ClaimTypes.Role, currentUser.UserRoles[0]) } :
                 new List<Claim> { new Claim(ClaimTypes.Name, login) };
            var roleClaim = (currentUser.UserRoles.Count != 0) ? new Claim(ClaimTypes.Role, currentUser.UserRoles[0].ToString()) : null;
            var nameClame = new Claim(ClaimTypes.Name, login);
            var identity = new ClaimsIdentity(new[] { roleClaim, nameClame }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            HttpContext.User = principal;
            var prop = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IssuedUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), prop);

            _logger.LogInformation($"Авторизация прошла успешно.\n " +
                $"Сообщение от api: {result.Content.ReadAsStringAsync().Result.UnescapeUnicode()}\n " +
                $"User.Identity.IsAuthenticated = {User.Identity.IsAuthenticated}\n" +
                $"HttpContext.User.Identity.Name = {HttpContext.User.Identity.Name}\n" +
                $"admin role={HttpContext.User.IsInRole("admin")}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var url = _httpClient.BaseAddress + "Accounts/logout";
            var result = _httpClient.PostAsync(url, null).Result;
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "Worker");
        }

        private bool CheckResult(HttpResponseMessage result)
        {
            if (!(result.StatusCode.ToString() == "OK"))
            {
                _logger.LogError($"Ошибка при обращении к апи result.StatusCode = {result.StatusCode}\n {result.Content.ReadAsStringAsync().Result.UnescapeUnicode()}");
                return false;
            }
            else return true;
        }
    }
}
