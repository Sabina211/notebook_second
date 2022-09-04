using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    public class WorkerData
    {
        private readonly DataContext _context;

        public WorkerData(DataContext context)
        {
            _context = context;
        }

        public string AddWorker(Worker worker)
        {
            var id = _context.Workers.Add(worker).Entity.Id.ToString();
            _context.SaveChanges();
            return id;
        }

        public void EditWorker(Worker worker)
        {
            var curentWorker = _context.Workers.ToList().Find(e => e.Id == worker.Id);
            curentWorker.Name = worker.Name;
            curentWorker.Surname = worker.Surname;
            curentWorker.Patronymic = worker.Patronymic;
            curentWorker.Address = worker.Address;
            curentWorker.PhoneNumber = worker.PhoneNumber;
            curentWorker.Description = worker.Description;
            _context.SaveChanges();
        }

        public void RemoveWorker(Worker worker)
        {
            _context.Workers.Remove(worker);
            _context.SaveChanges();
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return this._context.Workers;
        }
    }
}
