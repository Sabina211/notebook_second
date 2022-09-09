using ApiNotebook.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public class UserService :  IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ILogger<UserService> logger, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<string> ChangePassword(ChangePassword model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return null;
            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded) throw new Exception();
            _logger.LogInformation("\nИзменен пароль для пользователя с логином {0}, редактор {1}", user.UserName, "!исправить! User.Identity.Name");
            return "Пароль изменен";
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

        public async Task<string> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;
            await _userManager.DeleteAsync(user);
            _logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", user.UserName, "!исправить! User.Identity.Name");
            return "Пользователь удален";
        }

        public async Task<UserWithRolesEdit> EditUser(UserWithRolesEdit model)
        {
            User user = await _userManager.FindByIdAsync(model.Id.ToString());
            model.AllRoles = _roleManager.Roles.ToList();

            if (user == null) return null;

            user.Email = model.Email;
            // получем список ролей пользователя
            var oldUserRoles = await _userManager.GetRolesAsync(user);
            // получаем список ролей, которые были добавлены
            var addedRoles = model.UserRoles.Except(oldUserRoles);
            // получаем роли, которые были удалены
            var removedRoles = oldUserRoles.Except(model.UserRoles);

            var result1 = await _userManager.AddToRolesAsync(user, addedRoles);
            var result2 = await _userManager.RemoveFromRolesAsync(user, removedRoles);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded & result1.Succeeded & result2.Succeeded)
            {
                _logger.LogInformation("\nОтредактирован пользователь с логином {0}, редактор {1}", user.UserName, "!исправить! User.Identity.Name");
                return model;
            }
            throw new Exception();
        }

        public async Task<EditUser> EditUserEmail(EditUser model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return null;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return model;
        }

        public async Task<UserWithRolesEdit> GetCurrentUser(string id)
        {
            //Console.WriteLine(User.Identity.Name);
            //почему это не работает?
            //string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;
            UserWithRolesEdit userWithRoles = _mapper.Map<UserWithRolesEdit>(user);
            userWithRoles.UserRoles = await _userManager.GetRolesAsync(user);
            userWithRoles.AllRoles = _roleManager.Roles.ToList();
            return userWithRoles;
        }

        public async Task<UserWithRolesEdit> GetUser(Guid id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            UserWithRolesEdit userWithRoles = _mapper.Map<UserWithRolesEdit>(user);
            userWithRoles.UserRoles = await _userManager.GetRolesAsync(user);
            userWithRoles.AllRoles = _roleManager.Roles.ToList();
            return userWithRoles;
        }

        public async Task<IEnumerable<UserWithRolesEdit>> UsersList()
        {
            List<UserWithRolesEdit> users = new List<UserWithRolesEdit>();
            foreach (var item in _userManager.Users.ToList())
            {
                UserWithRolesEdit user = _mapper.Map<UserWithRolesEdit>(item);
                user.UserRoles = await _userManager.GetRolesAsync(item);
                user.AllRoles = _roleManager.Roles.ToList();
                users.Add(user);
            }
            return users;
        }
    }
}
