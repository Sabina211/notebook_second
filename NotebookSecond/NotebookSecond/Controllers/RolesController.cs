using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            /*User user = await _userManager.FindByIdAsync("181c2e0a-451e-4564-9582-8e5b4ed7fe02");
            var userRoles = await _userManager.GetRolesAsync(user);
            IdentityResult result = await _userManager.AddToRoleAsync(user, "admin");*/
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult UserList() => View(_userManager.Users.ToList());

    }
}
