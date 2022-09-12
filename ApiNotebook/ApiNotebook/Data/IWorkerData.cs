using ApiNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    public interface IWorkerData
    {
        Task Add(Worker worker);
    }
}
