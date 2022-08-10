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

namespace NotebookSecond.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> logger;
        private HttpClient httpClient { get; set; }

        public UsersController(ILogger<UsersController>? logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult UsersList(string? error)
        {
            string url = @"https://localhost:5005/api/Users/getUsers";
            string json = httpClient.GetStringAsync(url).Result;
            var users = JsonConvert.DeserializeObject<IEnumerable<UserWithRolesEdit>>(json);
            ViewData["Error"] = error;
            return View(users.ToList());
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public UserWithRolesEdit GetCurrentUser()
        {
            string url = @"https://localhost:5005/api/Users/getCurrentUser";
            string json = httpClient.GetStringAsync(url).Result;
            var currentUsers = JsonConvert.DeserializeObject<UserWithRolesEdit>(json);
            return currentUsers;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ViewCurrentUser()
        {
            var currentUser = GetCurrentUser();
            EditUser user = new EditUser { Id = currentUser.Id.ToString(), Login = currentUser.UserName, Email = currentUser.Email };
            return View(user);
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        [HttpGet]
        public IActionResult AddUser()
        {
            UserWithRoles userWithRoles = new UserWithRoles();
            userWithRoles.AllRoles = GetCurrentUser().AllRoles.ToList();
            return View(userWithRoles);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult AddUser(UserWithRoles userWithRoles)
        {
            userWithRoles.AllRoles = GetCurrentUser().AllRoles.ToList();
            var content = new StringContent(JsonConvert.SerializeObject(userWithRoles), Encoding.UTF8, "application/json");
            string url = @"https://localhost:5005/api/Users/addUser";
            if (!ModelState.IsValid)
                return View(userWithRoles);
            var result = httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, $"При добавлении пользователя произошла ошибка\n {result.Content.ReadAsStringAsync().Result}"); ;
                return View(userWithRoles);
            }
            try
            {
                UserWithRoles newUser = JsonConvert.DeserializeObject<UserWithRoles>(result.Content.ReadAsStringAsync().Result);
                logger.LogInformation("\nДобавлен новый пользователь с логином {0}, редактор {1}", userWithRoles.UserName, User.Identity.Name);
                return RedirectToAction("UsersList", "Users");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "При добавлении сотрудника не удалось преобразовать ответ в UserWithRoles");
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult EditUserEmail(EditUser model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            string url = @"https://localhost:5005/api/Users/editUserEmail";
            if (!(User.IsInRole("admin") || model.Login == User.Identity.Name))
                return Forbid();

            var result = httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, $"При редактировании почты пользователя произошла ошибка\n {result.Content.ReadAsStringAsync().Result}");
                return RedirectToAction("ViewCurrentUser", "Users");
            }
            var editedUser = JsonConvert.DeserializeObject<EditUser>(result.Content.ReadAsStringAsync().Result);
            logger.LogInformation("\nОтредактирована почта пользователя с логином {0}, редактор {1}", editedUser.Login, User.Identity.Name);
            return RedirectToAction("ViewCurrentUser", "Users");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult EditUser(UserWithRolesEdit model)
        {
            if (!ModelState.IsValid)
                return Redirect("/Users/UsersList?error= Error. User has not been edited");
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            string url = @"https://localhost:5005/api/Users/editUser";
            var result = httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
               
                return RedirectToAction("UsersList", "Users");
            }
            var editedUser = JsonConvert.DeserializeObject<UserWithRoles>(result.Content.ReadAsStringAsync().Result);
            logger.LogInformation("\nПользователь с логином {0} отредактирован, редактор {1}", editedUser.UserName, User.Identity.Name);
            return RedirectToAction("UsersList", "Users");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
        public IActionResult DeleteUser(EditUser model)
        {
            var uri = "https://localhost:5005/api/Users" + $"/{model.Id}";
            var result = httpClient.DeleteAsync(uri).Result;
            bool success = CheckResult(result);
            if (success)
            {
                logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", model.Login, User.Identity.Name);
                return RedirectToAction("UsersList", "Users");
            }
            return Redirect("/Users/UsersList?error= Error. User has not been deleted");
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ChangePassword()
        {
            var currentUser = GetCurrentUser();
            ChangePassword model = new ChangePassword { Id = currentUser.Id.ToString(), Login = currentUser.Email };
            return View(model);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            string url = @"https://localhost:5005/api/Users/changePassword";
            var result = httpClient.PostAsync(url, content).Result;
            bool success = CheckResult(result);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, $"При редактировании пароля произошла ошибка\n {result.Content.ReadAsStringAsync().Result}");
                return View(model);
            }
            logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", model.Login, User.Identity.Name);
            return View(model);
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
