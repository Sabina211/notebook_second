using ApiNotebook.Models;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public interface IAccountService
    {
        Task<UserWithRolesEdit> Login(LoginUser loginUser);
        Task<UserWithRolesEdit> Register(RegisterUser registerUser);
        Task Logout();
    }
}
