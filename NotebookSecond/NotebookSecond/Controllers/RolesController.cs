using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class RolesController : Controller
    {
        private HttpClient httpClient { get; set; }
        public RolesController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public IActionResult Index()
        {
            string url = @"https://localhost:5005/api/Roles";
            string json = httpClient.GetStringAsync(url).Result;
            var roles = JsonConvert.DeserializeObject<List<IdentityRole>>(json);
            return View(roles.ToList());
        }
    }
}
