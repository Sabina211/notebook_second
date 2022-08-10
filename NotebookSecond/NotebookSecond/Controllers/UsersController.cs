﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotebookSecond.Entities;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;

namespace NotebookSecond.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UsersController> logger;
        private HttpClient httpClient { get; set; }

        public UsersController( ILogger<UsersController>? logger, HttpClient httpClient)
        {
           /* this.userManager = userManager;
            this.roleManager = roleManager;*/
            this.logger = logger;
            this.httpClient = httpClient;
        }

    
        public UserWithRolesEdit GetCurrentUser()
        {
            string url = @"https://localhost:5005/api/Users/api/Users/getCurrentUser";
            /*var content = new StringContent(JsonConvert.SerializeObject(registerUser), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(url, content).Result;*/

            string json = httpClient.GetStringAsync(url).Result;
            
            var currentUsers = JsonConvert.DeserializeObject<UserWithRolesEdit>(json);

            return currentUsers;

        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> ViewCurrentUser()
        {
            var currentUser = await userManager.GetUserAsync(User);
            return View(currentUser);
        }

        //[Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddUser()
        {
            UserWithRoles userWithRoles = new UserWithRoles();
            userWithRoles.AllRoles = roleManager.Roles.ToList();
            return View(userWithRoles);
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(UserWithRoles userWithRoles)
        {
            userWithRoles.AllRoles = roleManager.Roles.ToList();
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = userWithRoles.Email,
                    UserName = userWithRoles.UserName
                };

                var result = await userManager.CreateAsync(user, userWithRoles.Password);
                //добавление роли пользователя
                await userManager.AddToRolesAsync(user, userWithRoles.UserRoles);

                if (result.Succeeded )
                {
                    logger.LogInformation("\nДобавлен новый пользователь с логином {0}, редактор {1}", userWithRoles.UserName, User.Identity.Name);
                    return RedirectToAction("UsersList", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(userWithRoles);
        }

        //[Authorize]
        public async Task<IActionResult> EditUser(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (User.IsInRole("admin") || user.UserName == User.Identity.Name)
            {
                if (user == null)
                {
                    return NotFound();
                }
                EditUser model = new EditUser { Id = user.Id, Email = user.Email };
                return View(model);
            }
            else
            {
                    return Forbid();
            }

        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> EditUser(UserWithRoles model)
        {
            if (User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                if (ModelState.IsValid)
                {
                    User user = await userManager.FindByIdAsync(model.Id);
                    model.AllRoles = roleManager.Roles.ToList();
                    //var allRoles = roleManager.Roles;
                    if (user != null)
                    {
                        user.Email = model.Email;
                        // получем список ролей пользователя
                        var oldUserRoles = await userManager.GetRolesAsync(user);
                        // получаем список ролей, которые были добавлены
                        var addedRoles = model.UserRoles.Except(oldUserRoles);
                        // получаем роли, которые были удалены
                        var removedRoles = oldUserRoles.Except(model.UserRoles);

                        var result1 = await userManager.AddToRolesAsync(user, addedRoles);
                        var result2 = await userManager.RemoveFromRolesAsync(user, removedRoles);

                        var result = await userManager.UpdateAsync(user);
                        if (result.Succeeded & result1.Succeeded & result2.Succeeded)
                        {
                            logger.LogInformation("\nОтредактирован пользователь с логином {0}, редактор {1}", user.UserName, User.Identity.Name);
                            if (User.IsInRole("admin"))
                            {
                                return RedirectToAction("UsersList", "Users");
                            }
                            else return RedirectToAction("ViewCurrentUser", "Users");                          
                        }
                        else
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, item.Description);
                            }
                            foreach (var item in result1.Errors)
                            {
                                ModelState.AddModelError(string.Empty, item.Description);
                            }
                            foreach (var item in result2.Errors)
                            {
                                ModelState.AddModelError(string.Empty, item.Description);
                            }
                        }
                    }
                }
                return RedirectToAction("UsersList", "Users");

            }
            else
            {
                return Forbid();
            }
            
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(EditUser model)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", user.UserName, User.Identity.Name);
            }
            return RedirectToAction("UsersList", "Users");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UsersList()
        {
            List<UserWithRoles> usersWithRoles = new List<UserWithRoles>();
            foreach (var item in userManager.Users.ToList())
            {
                UserWithRoles userWithRoles = new UserWithRoles();
                userWithRoles.Id = item.Id;
                userWithRoles.UserName = item.UserName;
                userWithRoles.Email = item.Email;
                userWithRoles.UserRoles = await userManager.GetRolesAsync(item);
                userWithRoles.AllRoles =  roleManager.Roles.ToList();
                usersWithRoles.Add(userWithRoles);
            }
            return View(usersWithRoles.ToList());
        }

        //[Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePassword model = new ChangePassword { Id = user.Id, Login = user.Email };
            return View(model);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        logger.LogInformation("\nИзменен пароль для пользователя с логином {0}, редактор {1}", user.UserName, User.Identity.Name);
                        return RedirectToAction("ViewCurrentUser", "Users");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }

    }
}
