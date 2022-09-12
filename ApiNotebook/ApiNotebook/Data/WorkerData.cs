using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    public class WorkerData : IWorkerData
    {
        private readonly DataContext _db;

        public WorkerData(DataContext db)
        {
            _db = db;
        }

        public async Task Add(Worker worker)
        {
            _db.Workers.Add(worker);
            await _db.SaveChangesAsync();
        }
    }
}
