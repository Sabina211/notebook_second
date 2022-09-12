using ApiNotebook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.BusinessLogic
{
    public interface IWorkerService
    {
        Task<Worker> Add(Worker worker);
    }
}
