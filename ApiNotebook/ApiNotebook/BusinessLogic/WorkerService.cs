using ApiNotebook.Data;
using ApiNotebook.Exceptions;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public class WorkerService : IWorkerService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(
            ILogger<WorkerService> logger,
            IHttpContextAccessor httpContextAccessor, 
            IWorkerRepository workerRepository)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _workerRepository = workerRepository;
        }

        public async Task<Worker> Add(Worker worker)
        {
            if (worker.Name == "admin")
                throw new Exception("Недопустимое имя сотрудника - admin");
            await _workerRepository.Add(worker);
            _logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", 
                worker.Name, 
                worker.Id,
                _httpContextAccessor.HttpContext.User.Identity.Name);
            return worker;
        }

        public async Task Delete(Guid id)
        {
            var worker = await _workerRepository.GetWorkerById(id);
            if (worker == null)
                throw new EntityNotFoundException($"Не удалось найти сотрудника с id {id}");
            await _workerRepository.Delete(worker);
            _logger.LogInformation("Удален сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, _httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task<Worker> Edit(Worker worker)
        {
            if (worker == null)
                throw new Exception("Сотрудник для редактирования не указан");
            if (await _workerRepository.GetWorkerById(worker.Id) == null)
                throw new EntityNotFoundException($"Не удалось найти сотрудника с id {worker.Id}");
            var result = await _workerRepository.Edit(worker);
            _logger.LogInformation("Отредактирован сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, _httpContextAccessor.HttpContext.User.Identity.Name);
            return result;
        }

        public async Task<Worker> GetWorkerById(Guid id)
        {
            var worker = await _workerRepository.GetWorkerById(id);
            if (worker == null)
                throw new EntityNotFoundException($"Не удалось найти сотрудника с  id {id}");
            return worker;
        }

        public async Task<IEnumerable<Worker>> GetWorkers()
        {
            return await _workerRepository.GetWorkers();
        }
    }
}
