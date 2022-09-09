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
        Task<UserWithRolesEdit> GetUser(Guid id);
        Task<UserWithRolesEdit> GetCurrentUser(string id);
        Task<EditUser> EditUserEmail(EditUser model);
        Task<UserWithRolesEdit> EditUser(UserWithRolesEdit model);
        Task<string> DeleteUser(string id);
        Task<IEnumerable<UserWithRolesEdit>> UsersList();
        Task<string> ChangePassword(ChangePassword model);

    }
}
