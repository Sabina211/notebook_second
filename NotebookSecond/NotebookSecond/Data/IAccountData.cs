using Microsoft.AspNetCore.Mvc;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public interface IAccountData
    {
        Task<bool> Login(LoginUser loginUser);
        Task<bool> Registration(RegisterUser registerUser);
        void Logout();
    }
}
