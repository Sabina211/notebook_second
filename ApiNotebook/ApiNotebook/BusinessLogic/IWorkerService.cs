using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public interface IWorkerService
    {
        Task<Worker> Add(Worker worker);
        Task<IEnumerable<Worker>> GetWorkers();
        Task<Worker> GetWorkerById(Guid id);
        Task<Worker> Edit(Worker worker);
        Task Delete(Guid id);
    }
}
