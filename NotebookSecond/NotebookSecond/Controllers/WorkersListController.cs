using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using NotebookSecond.ContextFolder;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Controllers
{
    public class WorkersListController : Controller
    {
        private readonly WorkerData workerData;
        public WorkersListController(WorkerData WorkerData)
        {
            workerData = WorkerData;
        }

        public IActionResult Index()
        {
            ViewBag.Workers = workerData.GetWorkers();
            ViewBag.Count = workerData.GetWorkers().Count();
            return View();
        }
    }
}
