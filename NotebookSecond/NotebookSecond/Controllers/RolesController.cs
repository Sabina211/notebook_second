using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NotebookSecond.Controllers
{
    public class RolesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RolesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            var httpClient = _httpClientFactory.CreateClient("httpClient");
            var url = httpClient.BaseAddress + "Roles";
            var json = httpClient.GetStringAsync(url).Result;
            var roles = JsonConvert.DeserializeObject<List<IdentityRole>>(json);
            return View(roles.ToList());
        }
    }
}
