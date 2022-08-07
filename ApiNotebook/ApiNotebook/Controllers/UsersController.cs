using ApiNotebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UsersController> logger;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UsersController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }


        [Route("api/[controller]/addUser")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesAdd>> AddUser(UserWithRolesAdd userWithRoles)
        {
            userWithRoles.AllRoles = roleManager.Roles.ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = new User
            {
                Email = userWithRoles.Email,
                UserName = userWithRoles.UserName
            };
            var result = await userManager.CreateAsync(user, userWithRoles.Password);

            //добавление роли пользователя
            await userManager.AddToRolesAsync(user, userWithRoles.UserRoles);

            if (result.Succeeded)
            {
                logger.LogInformation("\nДобавлен новый пользователь с логином {0}, редактор {1}", userWithRoles.UserName, User.Identity.Name);
                return Ok("Пользователь добавлен");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesEdit>> GetUser(Guid id)
        {
            User user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound("Пользователь с таким Id не найден");
            UserWithRolesEdit userWithRolesEdit = new UserWithRolesEdit
            {
                Id = id,
                UserName = user.UserName,
                Email = user.Email,
                UserRoles = await userManager.GetRolesAsync(user),
                AllRoles = roleManager.Roles.ToList()
            };
            return Ok(userWithRolesEdit);
        }

        [Route("api/[controller]/getCurrentUser")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserWithRolesEdit>> GetCurrentUser()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound("Пользователь с таким Id не найден");
            UserWithRolesEdit userWithRolesEdit = new UserWithRolesEdit
            {
                Id = Guid.Parse(user.Id),
                UserName = user.UserName,
                Email = user.Email,
                UserRoles = await userManager.GetRolesAsync(user),
            };
            return Ok(userWithRolesEdit);
        }

        [HttpPost]
        [Route("api/[controller]/editUserEmail")]
        [Authorize]
        public async Task<ActionResult<EditUser>> EditUserEmail(EditUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await userManager.FindByIdAsync(model.Id);
            if (User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                if (user == null)
                {
                    return NotFound("Пользователь с таким Id не найден");
                }
                user.Email = model.Email;
                var result = await userManager.UpdateAsync(user);
                return Ok("Почта сотрудника изменена");
            }
            else
            {
                return Forbid();
            }

        }

        [Route("api/[controller]/editUser")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesEdit>> EditUser(UserWithRolesEdit model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await userManager.FindByIdAsync(model.Id.ToString());
            model.AllRoles = roleManager.Roles.ToList();

            if (user == null) return NotFound("Пользователь с таким Id не найден");

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
                return Ok("Сотрудник обновлен");
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
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("id")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", user.UserName, User.Identity.Name);
                return Ok("Пользователь удален");
            }
            return NotFound("Пользователь с таким Id не найден");
        }

        [Route("api/[controller]/getUsers")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<UserWithRolesEdit>>> UsersList()
        {
            List<UserWithRolesEdit> users = new List<UserWithRolesEdit>();
            foreach (var item in userManager.Users.ToList())
            {
                UserWithRolesEdit user = new UserWithRolesEdit();
                user.Id = Guid.Parse(item.Id);
                user.UserName = item.UserName;
                user.Email = item.Email;
                user.UserRoles = await userManager.GetRolesAsync(item);
                user.AllRoles = roleManager.Roles.ToList();
                users.Add(user);
            }
            return users;
        }

        [Route("api/[controller]/changePassword")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!(User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Forbid();

            User user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                IdentityResult result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    logger.LogInformation("\nИзменен пароль для пользователя с логином {0}, редактор {1}", user.UserName, User.Identity.Name);
                    return Ok("Пароель изменен");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return NotFound("Пользователь не найден");
            }

        }
    }
}
