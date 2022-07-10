using Microsoft.AspNetCore.Mvc;
using NotebookSecond.ContextFolder;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotebookSecond.Controllers
{
    public class WorkerController : Controller
    {
        public IActionResult View(Guid? Id)
        {
            List<Worker> workers = new DataContext().Workers.ToList();
            var worker = workers.Find(e => e.Id == Id);
            return View(worker);
        }
        public IActionResult AddWorker()
        {
            return View();
        }

        //соханаем модель в БД с формы
        [HttpPost]
        public IActionResult GetWorkerFromViewDB(Worker worker)
        {
            using (var db = new DataContext())
            {
                var test = worker.Id;
                if (worker.Id != Guid.Empty)
                {
                    var curentWorker = db.Workers.Find(worker.Id);
                    curentWorker.Name = worker.Name;
                    curentWorker.Surname = worker.Surname;
                    curentWorker.Patronymic = worker.Patronymic;
                    curentWorker.Address = worker.Address;
                    curentWorker.PhoneNumber = worker.PhoneNumber;
                    curentWorker.Description = worker.Description;
                }
                else
                {
                    db.Workers.Add(new Worker()
                    {
                        Name = worker.Name,
                        Surname = worker.Surname,
                        Patronymic = worker.Patronymic,
                        PhoneNumber = worker.PhoneNumber,
                        Address = worker.Address,
                        Description = worker.Description
                    });
                }
                db.SaveChanges();
            }
            return Redirect("/WorkersList/Index");
        }

        [HttpPost]
        public IActionResult DeleteWorkerFromViewDB(Worker worker)
        {
            using (var db = new DataContext())
            {
                var curentWorker = db.Workers.Find(worker.Id);
                db.Workers.Remove(curentWorker);
                db.SaveChanges();
            }
            return Redirect("/WorkersList/Index");
        }
    }
}
