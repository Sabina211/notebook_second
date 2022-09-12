using ApiNotebook.BusinessLogic;
using ApiNotebook.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _accountService = accountService;
        }

        [Route("api/[controller]/register")]
        [HttpPost]
        public async Task<ActionResult<RegisterUser>> Post(RegisterUser registerUser)
        {
            if (registerUser.Login == "admin")
            {
                ModelState.AddModelError("Login", "Недопустимое имя пользователя - admin");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = new User
            {
                Email = registerUser.Email,
                UserName = registerUser.Login
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                UserWithRolesEdit userWithRolesEdit = new UserWithRolesEdit
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    Email = user.Email,
                    UserRoles = await _userManager.GetRolesAsync(user),
                };
                return Ok(userWithRolesEdit);
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

        [AllowAnonymous]
        [Route("api/[controller]/login")]
        [HttpPost]
        public async Task<ActionResult<UserWithRolesEdit>> Post(LoginUser loginUser)
        {
            /*var result = await _signInManager.PasswordSignInAsync(loginUser.Login, loginUser.Password, false, false);
            if (result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(loginUser.Login);
                Console.WriteLine($" авторизован как {User.Identity.Name}");

                if (user == null) return NotFound("Пользователь с таким Id не найден");
                UserWithRolesEdit userWithRolesEdit = _mapper.Map<UserWithRolesEdit>(user);
                userWithRolesEdit.UserRoles = await _userManager.GetRolesAsync(user);
                return Ok(userWithRolesEdit);
            }
            else
            {
                ModelState.AddModelError("", "Некорректный логин и/или пароль");
                return BadRequest(ModelState);
            }*/
            var result = await _accountService.Login(loginUser);
            if (result == null) return BadRequest("Некорректный логин и/или пароль");
            return Ok(result);
        }

        [HttpPost]
        [Route("api/[controller]/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
