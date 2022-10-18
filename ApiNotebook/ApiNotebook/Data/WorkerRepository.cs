using ApiNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly DataContext _db;

        public WorkerRepository(DataContext db)
        {
            _db = db;
        }

        public async Task Add(Worker worker)
        {
            _db.Workers.Add(worker);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Worker worker)
        {
            _db.Workers.Remove(worker);
            await _db.SaveChangesAsync();
        }

        public async Task<Worker> Edit(Worker worker)
        {
            _db.Update(worker);
            await _db.SaveChangesAsync();
            var result = await GetWorkerById(worker.Id);
            return result;
        }

        public async Task<Worker> GetWorkerById(Guid id)
        {
            var worker = await _db.Workers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return worker;
        }

        public async Task<IEnumerable<Worker>> GetWorkers()
        {
            return await _db.Workers.AsNoTracking().ToListAsync();
        }
    }
}
