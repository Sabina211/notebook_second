﻿using ApiNotebook.Data;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly ILogger<WorkersController> _logger;
        public WorkersController(DataContext context, ILogger<WorkersController> logger)
        {
            _db = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> Get()
        {
            return await _db.Workers.ToListAsync();
        }

        // GET api/Workers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> Get(string id)
        {
            Worker worker = await _db.Workers.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (worker == null)
                return NotFound();
            return new ObjectResult(worker);
        }

        // POST api/Workers
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Worker>> Post([FromBody] Worker worker)
        {
            if (worker.Name == "admin")
            {
                ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Workers.Add(worker);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return worker;
        }

        // PUT api/Workers/
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Worker>> Put(Worker worker)
        {
            if (worker == null)
            {
                return BadRequest();
            }
            if (!_db.Workers.Any(x => x.Id == worker.Id))
            {
                return NotFound();
            }

            _db.Update(worker);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Отредактирован сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Ok(worker);
        }

        // DELETE api/Workers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Worker>> Delete(string id)
        {
            Worker worker = _db.Workers.FirstOrDefault(x => x.Id.ToString() == id);
            if (worker == null)
            {
                return NotFound(new { message = "Сотрудник с таким Id не найден" });
            }
            _db.Workers.Remove(worker);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Удален сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Ok(worker);
        }
    }
}
