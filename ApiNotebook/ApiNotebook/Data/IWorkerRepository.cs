using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    public interface IWorkerRepository
    {
        Task Add(Worker worker);
        Task<Worker> GetWorkerById(Guid id);
        Task<IEnumerable<Worker>> GetWorkers();
        Task<Worker> Edit(Worker worker);
        Task Delete(Worker worker);
    }
}
