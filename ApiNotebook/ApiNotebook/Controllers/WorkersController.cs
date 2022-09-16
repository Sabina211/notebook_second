using ApiNotebook.BusinessLogic;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        public WorkersController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
            return Ok(await _workerService.GetWorkers());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Worker>> GetWorkerById(Guid id)
        {
            var worker = await _workerService.GetWorkerById(id);
            return Ok(worker);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Worker>> Add([FromBody] Worker worker)
        {
            var result = await _workerService.Add(worker);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Worker>> Edit(Worker worker)
        {
            return Ok(await _workerService.Edit(worker));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Worker>> Delete(Guid id)
        {
            await _workerService.Delete(id);
            return Ok();
        }
    }
}
