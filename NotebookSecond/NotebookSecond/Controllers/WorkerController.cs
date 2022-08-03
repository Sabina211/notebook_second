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
        private readonly ILogger<WorkerController> logger;

        public WorkerController(WorkerData WorkerData, ILogger<WorkerController> logger)
        {
            this.workerData = WorkerData;
            this.logger = logger;
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
            return View();
        }

        //соханаем модель в БД с формы
        [HttpPost]
        [Authorize]
        public IActionResult GetWorkerFromViewDB(Worker worker)
        {
            if (worker.Id != Guid.Empty)
            {
                workerData.EditWorker(worker);
                logger.LogInformation("Отредактирован сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            }
            else
            {
                var id = workerData.AddWorker(new Worker()
                {
                    Name = worker.Name,
                    Surname = worker.Surname,
                    Patronymic = worker.Patronymic,
                    PhoneNumber = worker.PhoneNumber,
                    Address = worker.Address,
                    Description = worker.Description
                });
                logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", worker.Name, id, User.Identity.Name);
            }

            return Redirect("/WorkersList/Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteWorkerFromViewDB(Worker worker)
        {
            var curentWorker = workerData.GetWorkers().ToList().Find(e => e.Id == worker.Id);
            workerData.RemoveWorker(curentWorker);
            logger.LogInformation("Удален сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Redirect("/WorkersList/Index");
        }
    }
}
