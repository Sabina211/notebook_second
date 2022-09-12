using ApiNotebook.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager = null, IMapper mapper = null, ILogger<AccountService> logger = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserWithRolesEdit> Login(LoginUser loginUser)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Login, loginUser.Password, false, false);
            if (!result.Succeeded) return null;
            User user = await _userManager.FindByNameAsync(loginUser.Login);
            UserWithRolesEdit userWithRolesEdit = _mapper.Map<UserWithRolesEdit>(user);
            userWithRolesEdit.UserRoles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation($"Выполнен вход в систему от {loginUser.Login}");
            return userWithRolesEdit;
        }
    }
}
