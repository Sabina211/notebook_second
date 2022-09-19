using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using NotebookSecond.Data;

namespace NotebookSecond.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public UsersController(ILogger<UsersController>? logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("httpClient");
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult UsersList(string? error)
        {
            var url = _httpClient.BaseAddress + "Users";
            var json = _httpClient.GetStringAsync(url).Result;
            var users = JsonConvert.DeserializeObject<IEnumerable<UserWithRolesEdit>>(json);
            ViewData["Error"] = error;
            return View(users.ToList());
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public UserWithRolesEdit GetCurrentUser()
        {
            var url = _httpClient.BaseAddress + "Users/current";
            var json = _httpClient.GetStringAsync(url).Result;
            var currentUsers = JsonConvert.DeserializeObject<UserWithRolesEdit>(json);
            return currentUsers;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ViewCurrentUser()
        {
            var currentUser = GetCurrentUser();
            var user = new EditUser { Id = currentUser.Id.ToString(), Login = currentUser.UserName, Email = currentUser.Email };
            return View(user);
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        [HttpGet]
        public IActionResult AddUser()
        {
            var userWithRoles = new UserWithRoles();
            userWithRoles.AllRoles = GetCurrentUser().AllRoles.ToList();
            return View(userWithRoles);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public async Task<IActionResult> AddUserAsync(UserWithRoles userWithRoles)
        {
            userWithRoles.AllRoles = GetCurrentUser().AllRoles.ToList();
            var content = new StringContent(JsonConvert.SerializeObject(userWithRoles), Encoding.UTF8, "application/json");
            var url = _httpClient.BaseAddress + "Users";
            if (!ModelState.IsValid)
                return View(userWithRoles);
            var result = _httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
                var errorText =  result.Content.ReadAsStringAsync().Result.UnescapeUnicode();
                ModelState.AddModelError(string.Empty, $"При добавлении пользователя произошла ошибка\n {errorText}"); ;
                return View(userWithRoles);
            }
            try
            {
                var newUser = JsonConvert.DeserializeObject<UserWithRoles>(result.Content.ReadAsStringAsync().Result);
                _logger.LogInformation("\nДобавлен новый пользователь с логином {0}, редактор {1}", userWithRoles.UserName, User.Identity.Name);
                return RedirectToAction("UsersList", "Users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При добавлении сотрудника не удалось преобразовать ответ в UserWithRoles");
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult EditUserEmail(EditUser model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var url = _httpClient.BaseAddress + "Users/email";
            if (!(User.IsInRole("admin") || model.Login == User.Identity.Name))
                return Forbid();

            var result = _httpClient.PatchAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, $"При редактировании почты пользователя произошла ошибка\n {result.Content.ReadAsStringAsync().Result.UnescapeUnicode()}");
                return RedirectToAction("ViewCurrentUser", "Users");
            }
            var editedUser = JsonConvert.DeserializeObject<EditUser>(result.Content.ReadAsStringAsync().Result);
            _logger.LogInformation("\nОтредактирована почта пользователя с логином {0}, редактор {1}", editedUser.Login, User.Identity.Name);
            return RedirectToAction("ViewCurrentUser", "Users");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult EditUser(UserWithRolesEdit model)
        {
            if (!ModelState.IsValid)
                return Redirect("/Users/UsersList?error= Error. User has not been edited");
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var url = _httpClient.BaseAddress + "Users";
            var result = _httpClient.PutAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            { 
                return RedirectToAction("UsersList", "Users");
            }
            var editedUser = JsonConvert.DeserializeObject<UserWithRoles>(result.Content.ReadAsStringAsync().Result);
            _logger.LogInformation("\nПользователь с логином {0} отредактирован, редактор {1}", editedUser.UserName, User.Identity.Name);
            return RedirectToAction("UsersList", "Users");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult DeleteUser(EditUser model)
        {
            var url = _httpClient.BaseAddress + $"Users/{model.Id}";
            var result = _httpClient.DeleteAsync(url).Result;
            var success = CheckResult(result);
            if (success)
            {
                _logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", model.Login, User.Identity.Name);
                return RedirectToAction("UsersList", "Users");
            }
            return Redirect("/Users/UsersList?error= Error. User has not been deleted");
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ChangePassword()
        {
            var currentUser = GetCurrentUser();
            var model = new ChangePassword { Id = currentUser.Id.ToString(), Login = currentUser.Email };
            return View(model);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var url = _httpClient.BaseAddress + "Users/password";
            var result = _httpClient.PatchAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, $"При редактировании пароля произошла ошибка\n {result.Content.ReadAsStringAsync().Result.UnescapeUnicode()}");
                return View(model);
            }
            _logger.LogInformation("\nИзменен пароль пользователя {0}, редактор {1}", model.Login, User.Identity.Name);
            return View(model);
        }

        private bool CheckResult(HttpResponseMessage result)
        {
            if (!(result.StatusCode.ToString() == "OK"))
            {
                _logger.LogError($"Ошибка при обращении к апи result.StatusCode = {result.StatusCode}\n {result.Content.ReadAsStringAsync().Result}");
                return false;
            }
            else return true;
        }

    }
}
