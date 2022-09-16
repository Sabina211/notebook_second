using ApiNotebook.BusinessLogic;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApiNotebook.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserWithRolesEdit>> Register(RegisterUser registerUser)
        {
            var user = await _accountService.Register(registerUser);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithRolesEdit>> Login(LoginUser loginUser)
        {
            var result = await _accountService.Login(loginUser);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return Ok();
        }
    }
}
