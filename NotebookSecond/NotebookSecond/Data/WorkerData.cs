using NotebookSecond.ContextFolder;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public class WorkerData : IWorkerData
    {
        private readonly DataContext Context;

        public WorkerData(DataContext Context)
        {
            this.Context = Context;
        }

        public Worker AddWorker(Worker worker)
        {
            var newWorker = Context.Workers.Add(worker).Entity;
            Context.SaveChanges();
            return newWorker;
        }

        public Worker EditWorker(Worker worker)
        {
            var curentWorker = Context.Workers.ToList().Find(e => e.Id == worker.Id);
            curentWorker.Name = worker.Name;
            curentWorker.Surname = worker.Surname;
            curentWorker.Patronymic = worker.Patronymic;
            curentWorker.Address = worker.Address;
            curentWorker.PhoneNumber = worker.PhoneNumber;
            curentWorker.Description = worker.Description;
            Context.SaveChanges();
            return curentWorker;
        }

        public bool RemoveWorker(Worker worker)
        {
            var result = Context.Workers.Remove(worker);
            Context.SaveChanges();
            return true;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return this.Context.Workers;
        }
    }
}
