using ApiNotebook.BusinessLogic;
using ApiNotebook.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApiNotebook.Controllers
{
    [ApiController]
    [Route("api/accounts")]
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
        
        [HttpPost("register")]
        public async Task<ActionResult<RegisterUser>> Register(RegisterUser registerUser)
        {
            if (registerUser.Login == "admin")
            {
                ModelState.AddModelError("Login", "Недопустимое имя пользователя - admin");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
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

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithRolesEdit>> Login(LoginUser loginUser)
        {
            var result = await _accountService.Login(loginUser);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("api/[controller]/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
