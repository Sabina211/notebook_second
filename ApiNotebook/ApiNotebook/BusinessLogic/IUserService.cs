using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public interface IUserService
    {
        Task<UserWithRolesAdd> Create(UserWithRolesAdd user);
        Task<UserWithRolesEdit> GetUserById(Guid id);
        Task<UserWithRolesEdit> GetCurrentUser();
        Task<EditUser> EditUserEmail(EditUser model);
        Task<UserWithRolesEdit> EditUser(UserWithRolesEdit model);
        Task DeleteUser(string id);
        Task<IEnumerable<UserWithRolesEdit>> UsersList();
        Task ChangePassword(ChangePassword model);

    }
}
