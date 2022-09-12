using ApiNotebook.Data;
using ApiNotebook.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerData _workerData;
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(IWorkerData workerData, ILogger<WorkerService> logger)
        {
            _workerData = workerData;
            _logger = logger;
        }

        public async Task<Worker> Add(Worker worker)
        {
            if (worker.Name == "admin")
            {
                //ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
                return null;
            }

            await _workerData.Add(worker);
            _logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, $"!!Изменить User.Identity.Name");
            return worker;
        }
    }
}
