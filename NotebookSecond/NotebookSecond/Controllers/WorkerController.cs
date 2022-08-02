using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly WorkerData workerData;
        private readonly ILogger<WorkerController> _logger;

        public WorkerController(WorkerData WorkerData, ILogger<WorkerController> logger)
        {
            this.workerData = WorkerData;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }
        public IActionResult View(Guid? Id)
        {
            List<Worker> workers = workerData.GetWorkers().ToList();
            var worker = workers.Find(e => e.Id == Id);
            return View(worker);
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult AddWorker()
        {
            _logger.LogDebug("Открыта страница с добавлением сотрудника");
            return View();
        }

        //соханаем модель в БД с формы
        [HttpPost]
        [Authorize]
        public IActionResult GetWorkerFromViewDB(Worker worker)
        {
            var test = worker.Id;
            if (worker.Id != Guid.Empty)
            {
                workerData.EditWorker(worker);
            }
            else
            {
                workerData.AddWorker(new Worker()
                {
                    Name = worker.Name,
                    Surname = worker.Surname,
                    Patronymic = worker.Patronymic,
                    PhoneNumber = worker.PhoneNumber,
                    Address = worker.Address,
                    Description = worker.Description
                });
            }
            return Redirect("/WorkersList/Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteWorkerFromViewDB(Worker worker)
        {
            var curentWorker = workerData.GetWorkers().ToList().Find(e => e.Id == worker.Id);
            workerData.RemoveWorker(curentWorker);
            return Redirect("/WorkersList/Index");
        }
    }
}
