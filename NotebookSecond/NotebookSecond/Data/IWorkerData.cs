using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public interface IWorkerData
    {
        IEnumerable<Worker> GetWorkers();
        Worker AddWorker(Worker worker);
        Worker EditWorker(Worker worker);
        bool RemoveWorker(Worker worker);


    }
}
