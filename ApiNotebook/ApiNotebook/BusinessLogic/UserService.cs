using ApiNotebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ILogger<UserService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<UserWithRolesAdd> Create(UserWithRolesAdd userWithRoles)
        {
            userWithRoles.AllRoles = _roleManager.Roles.ToList();
            User user = new User
            {
                Email = userWithRoles.Email,
                UserName = userWithRoles.UserName
            };
            var result = await _userManager.CreateAsync(user, userWithRoles.Password);

            //добавление роли пользователя
            await _userManager.AddToRolesAsync(user, userWithRoles.UserRoles);

            if (!result.Succeeded) throw new Exception();
            _logger.LogInformation("\nДобавлен новый пользователь с логином {0}", userWithRoles.UserName);
            return userWithRoles;

        }
    }
}
