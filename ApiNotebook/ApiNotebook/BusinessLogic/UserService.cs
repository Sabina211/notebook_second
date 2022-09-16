using ApiNotebook.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ApiNotebook.Exceptions;

namespace ApiNotebook.BusinessLogic
{
    public class UserService :  IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            RoleManager<IdentityRole> roleManager, 
            UserManager<User> userManager, 
            ILogger<UserService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ChangePassword(ChangePassword model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) throw new EntityNotFoundException($"Пользователь с id = {model.Id} не найден");
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded) throw new Exception("Не удалось изменить пароль");
            _logger.LogInformation("\nИзменен пароль для пользователя с логином {0}, редактор {1}", 
                user.UserName, 
                _httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task<UserWithRolesAdd> Create(UserWithRolesAdd userWithRoles)
        {
            userWithRoles.AllRoles = _roleManager.Roles.ToList();
            var user = new User
            {
                Email = userWithRoles.Email,
                UserName = userWithRoles.UserName
            };
            var result = await _userManager.CreateAsync(user, userWithRoles.Password);

            //добавление роли пользователя
            await _userManager.AddToRolesAsync(user, userWithRoles.UserRoles);

            if (!result.Succeeded) throw new Exception($"Не удалось создать пользователя: {userWithRoles.Email}");
            _logger.LogInformation("\nДобавлен новый пользователь с логином {0}, редактор: {1}", userWithRoles.UserName, _httpContextAccessor.HttpContext.User.Identity.Name);
            return userWithRoles;
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new EntityNotFoundException($"Пользователь с id = {id} не найден");
            await _userManager.DeleteAsync(user);
            _logger.LogInformation("\nУдален пользователь с логином {0}, редактор {1}", user.UserName, _httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task<UserWithRolesEdit> EditUser(UserWithRolesEdit model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            model.AllRoles = _roleManager.Roles.ToList();

            if (user == null) throw new EntityNotFoundException($"Пользователь с id = {model.Id} не найден");

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
            throw new Exception($"Не удалось изменить пользователя");
        }

        public async Task<EditUser> EditUserEmail(EditUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) throw new EntityNotFoundException($"Entity user with id: {model.Id} not found");
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return model;
        }

        public async Task<UserWithRolesEdit> GetCurrentUser()
        {
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new EntityNotFoundException($"Entity user with id: {id} not found");
            var userWithRoles = _mapper.Map<UserWithRolesEdit>(user);
            userWithRoles.UserRoles = await _userManager.GetRolesAsync(user);
            userWithRoles.AllRoles = _roleManager.Roles.ToList();
            return userWithRoles;
        }

        public async Task<UserWithRolesEdit> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new EntityNotFoundException($"Entity user with id: {id} not found");            
            var userWithRoles = _mapper.Map<UserWithRolesEdit>(user);
            userWithRoles.UserRoles = await _userManager.GetRolesAsync(user);
            userWithRoles.AllRoles = _roleManager.Roles.ToList();
            return userWithRoles;
        }

        public async Task<IEnumerable<UserWithRolesEdit>> UsersList()
        {
            var users = new List<UserWithRolesEdit>();           
            foreach (var item in _userManager.Users.ToList())
            {
                var user = _mapper.Map<UserWithRolesEdit>(item);
                user.UserRoles = await _userManager.GetRolesAsync(item);
                user.AllRoles = _roleManager.Roles.ToList();
                users.Add(user);
            }          
            return users;
        }
    }
}
