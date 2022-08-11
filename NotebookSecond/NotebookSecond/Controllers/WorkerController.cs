﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NotebookSecond.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkerData workerData;
        private readonly ILogger<WorkerController> logger;
        private HttpClient httpClient { get; set; }

        public WorkerController(IWorkerData WorkerData, ILogger<WorkerController> logger)
        {
            httpClient = new HttpClient();
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

        public IActionResult Description()
        {
            return View();
        }

        public IActionResult View(Guid? Id)
        {
            List<Worker> workers = workerData.GetWorkers().ToList();
            var worker = workers.Find(e => e.Id == Id);
            return View(worker);
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult AddWorker()
        {
            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
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
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult EditWorker(Worker worker)
        {
            Worker editedWorker= workerData.EditWorker(worker);
            if (editedWorker.Id == Guid.Empty)
                return Redirect("/Worker/Index?error= Error. Employee has not been edited");
            logger.LogInformation("Отредактирован сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Redirect("/Worker/Index");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "admin")]
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
