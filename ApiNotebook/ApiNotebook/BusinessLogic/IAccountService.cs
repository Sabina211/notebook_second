using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public interface IAccountService
    {
        Task<UserWithRolesEdit> Login(LoginUser loginUser);
    }
}
