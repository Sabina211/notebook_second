using ApiNotebook.BusinessLogic;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="userWithRoles"></param>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesAdd>> AddUser(UserWithRolesAdd userWithRoles)
        {
            var result = await _userService.Create(userWithRoles);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesEdit>> GetUserById(Guid id)
        {
            var result = await _userService.GetUserById(id);               
            return Ok(result);
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        [HttpGet("current")]
        [Authorize]
        public async Task<ActionResult<UserWithRolesEdit>> GetCurrentUser()
        {
            var result = await _userService.GetCurrentUser();
            return Ok(result); 
        }

        [HttpPatch("email")]
        [Authorize]
        public async Task<ActionResult<EditUser>> EditUserEmail(EditUser model)
        {
            if (!(User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Forbid();
            var result = await _userService.EditUserEmail(model);
            return Ok(result); 
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserWithRolesEdit>> EditUser(UserWithRolesEdit model)
        {
            var result = await _userService.EditUser(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }

        /// <summary>
        /// Список пользователей
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<UserWithRolesEdit>> UsersList()
        {
            return await _userService.UsersList();
        }

        [HttpPatch("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!(User.IsInRole("admin") || model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Forbid();
            await _userService.ChangePassword(model);
            return Ok();
        }
    }
}
