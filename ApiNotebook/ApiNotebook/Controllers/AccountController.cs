using ApiNotebook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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

            var result = await userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return Ok($"Пользователь {registerUser.Login} зерегистрирован");
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

       // [ValidateAntiForgeryToken]
        [Route("api/[controller]/login")]
        [HttpPost]
        public async Task<ActionResult<LoginUser>> Post(LoginUser loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await signInManager.PasswordSignInAsync(loginUser.Login, loginUser.Password, false, false);
            if (result.Succeeded)
            {
                return Ok($"Пользователь {loginUser.Login} авторизован");
            }
            else
            {
                ModelState.AddModelError("", "Некорректный логин и/или пароль");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("api/[controller]/logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
