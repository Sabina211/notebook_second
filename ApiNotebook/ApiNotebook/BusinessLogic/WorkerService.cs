using ApiNotebook.Data;
using ApiNotebook.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public class WorkerService : IWorkerService
    {
        private readonly DataContext _context;
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(ILogger<WorkerService> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Worker> Add(Worker worker)
        {
            if (worker.Name == "admin")
            {
                //ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
                return null;
            }

            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, $"!!Изменить User.Identity.Name");
            return worker;
        }
    }
}
