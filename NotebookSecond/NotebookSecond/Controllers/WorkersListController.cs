using Microsoft.AspNetCore.Mvc;
using NotebookSecond.ContextFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class WorkersListController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Workers = new DataContext().Workers;          
            return View();
        }
        public IActionResult IndexTest()
        {
            ViewBag.Workers = new DataContext().Workers;
            ViewBag.Count = new DataContext().Workers.Count();
            return View();
        }
    }
}
