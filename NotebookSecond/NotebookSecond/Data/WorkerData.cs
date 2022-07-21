using NotebookSecond.ContextFolder;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public class WorkerData
    {
        private readonly DataContext Context;
        //static List<Worker> Workers;

        public WorkerData(DataContext Context)
        {
            this.Context = Context;
            //Workers = this.Context.Workers.ToList();
        }

        public void AddWorker(Worker worker)
        {
            Context.Workers.Add(worker);
            Context.SaveChanges();
        }

        public void EditWorker(Worker worker)
        {
            var curentWorker = Context.Workers.ToList().Find(e => e.Id == worker.Id);
            curentWorker.Name = worker.Name;
            curentWorker.Surname = worker.Surname;
            curentWorker.Patronymic = worker.Patronymic;
            curentWorker.Address = worker.Address;
            curentWorker.PhoneNumber = worker.PhoneNumber;
            curentWorker.Description = worker.Description;
            Context.SaveChanges();
        }

        public void RemoveWorker(Worker worker)
        {
            Context.Workers.Remove(worker);
            Context.SaveChanges();
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return this.Context.Workers;
        }
    }
}
