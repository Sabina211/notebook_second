using ApiNotebook.BusinessLogic;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("~/api/[controller]/addUser")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesAdd>> AddUser(UserWithRolesAdd userWithRoles)
        {
            var result = await _userService.Create(userWithRoles);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesEdit>> GetUser(Guid id)
        {
            var result = await _userService.GetUser(id);
            if (result == null) return NotFound("Пользователь с таким Id не найден");
            return Ok(result);
        }

        [Route("~/api/[controller]/getCurrentUser")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserWithRolesEdit>> GetCurrentUser()
        {
            Console.WriteLine($"User {User.Identity.Name}");
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.GetCurrentUser(id);
            if(result==null) return NotFound("Пользователь с таким Id не найден");
            return Ok(result); 
        }

        [HttpPost]
        [Route("~/api/[controller]/editUserEmail")]
        [Authorize]
        public async Task<ActionResult<EditUser>> EditUserEmail(EditUser model)
        {
            if (User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                var result = await _userService.EditUserEmail(model);
                if (result == null) return NotFound("Пользователь с таким Id не найден");              
                return Ok(result);
            }
            else return Forbid();
        }

        [Route("~/api/[controller]/editUser")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesEdit>> EditUser(UserWithRolesEdit model)
        {
            var result = await _userService.EditUser(model);
            if (result == null) return NotFound("Пользователь с таким Id не найден");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            var result = await _userService.DeleteUser(id);
            if (result == null) return NotFound("Пользователь с таким Id не найден");
            return Ok(result);
        }

        [Route("~/api/[controller]/getUsers")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<UserWithRolesEdit>> UsersList()
        {
            return await _userService.UsersList();
        }

        [Route("~/api/[controller]/changePassword")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!(User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Forbid();
            var result = await _userService.ChangePassword(model);
            if(result==null)  return NotFound("Пользователь не найден");
            return Ok(result);
        }
    }
}
