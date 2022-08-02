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
        private readonly ILogger<WorkersListController> _logger;
        //private static Logger logger2 = LogManager.GetCurrentClassLogger();
       // private readonly Logger logger2;
        public WorkersListController( ILogger<WorkersListController> logger, WorkerData WorkerData)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
            _logger.LogError("Hello1");
            workerData = WorkerData;

        }

        public IActionResult Index()
        {
            ViewBag.Workers = workerData.GetWorkers();
            ViewBag.Count = workerData.GetWorkers().Count();
            //не пишутся


            //logger2.Error("Текстовая ошибка");

            return View();
        }

        /*public IActionResult IndexTest()
        {
            ViewBag.Workers = new DataContext().Workers;
            ViewBag.Count = new DataContext().Workers.Count();
            return View();
        }*/
    }
}
