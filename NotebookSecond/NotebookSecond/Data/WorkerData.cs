using NotebookSecond.ContextFolder;
using NotebookSecond.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NotebookSecond.Data
{
    public class WorkerData : IWorkerData
    {
        private readonly DataContext _context;

        public WorkerData(DataContext context)
        {
            _context = context;
        }

        public Worker AddWorker(Worker worker)
        {
            var newWorker = _context.Workers.Add(worker).Entity;
            _context.SaveChanges();
            return newWorker;
        }

        public Worker EditWorker(Worker worker)
        {
            var curentWorker = _context.Workers.ToList().Find(e => e.Id == worker.Id);
            curentWorker.Name = worker.Name;
            curentWorker.Surname = worker.Surname;
            curentWorker.Patronymic = worker.Patronymic;
            curentWorker.Address = worker.Address;
            curentWorker.PhoneNumber = worker.PhoneNumber;
            curentWorker.Description = worker.Description;
            _context.SaveChanges();
            return curentWorker;
        }

        public bool RemoveWorker(Worker worker)
        {
            var result = _context.Workers.Remove(worker);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return this._context.Workers;
        }
    }
}
