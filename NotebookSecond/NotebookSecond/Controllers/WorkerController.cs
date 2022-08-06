using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NotebookSecond.ContextFolder;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotebookSecond.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkerData workerData;
        private readonly ILogger<WorkerController> logger;

        public WorkerController(IWorkerData WorkerData, ILogger<WorkerController> logger)
        {
            this.workerData = WorkerData;
            this.logger = logger;
        }

        public IActionResult Index(string? error)
        {
            ViewBag.Workers = workerData.GetWorkers();
            ViewBag.Count = workerData.GetWorkers().Count();
            ViewData["Error"] = error;
            return View();
        }

        public IActionResult View(Guid? Id)
        {
            List<Worker> workers = workerData.GetWorkers().ToList();
            var worker = workers.Find(e => e.Id == Id);
            return View(worker);
        }

        //[Authorize]
        [HttpGet]
        public IActionResult AddWorker()
        {
            return View();
        }

        [HttpPost]
        //[Authorize]
        public IActionResult AddWorker(Worker worker)
        {
            if (!ModelState.IsValid)
                return View(worker);
            Worker newWorker = workerData.AddWorker(new Worker()
            {
                Name = worker.Name,
                Surname = worker.Surname,
                Patronymic = worker.Patronymic,
                PhoneNumber = worker.PhoneNumber,
                Address = worker.Address,
                Description = worker.Description
            });
            if (newWorker.Id == Guid.Empty)
                return Redirect("/Worker/Index?error= Error. Employee has not been added");

            logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", newWorker.Name, newWorker.Id, User.Identity.Name);
            return Redirect("/Worker/Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditWorker(Worker worker)
        {
            Worker editedWorker= workerData.EditWorker(worker);
            if (editedWorker.Id == Guid.Empty)
                return Redirect("/Worker/Index?error= Error. Employee has not been edited");
            logger.LogInformation("Отредактирован сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Redirect("/Worker/Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteWorkerFromViewDB(Worker worker)
        {
            var curentWorker = workerData.GetWorkers().ToList().Find(e => e.Id == worker.Id);
            bool success = workerData.RemoveWorker(curentWorker);
            if (!success)
                return Redirect("/Worker/Index?error= Error. Employee has not been deleted");
            logger.LogInformation("Сотрудник {0} c id={1} был удален, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Redirect("/Worker/Index");
        }
    }
}
