using ApiNotebook.Data;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiNotebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        DataContext db;
        private readonly ILogger<WorkersController> logger;
        public WorkersController(DataContext context, ILogger<WorkersController> logger)
        {
            db = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> Get()
        {
            return await db.Workers.ToListAsync();
        }

        // GET api/Workers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> Get(string id)
        {
            Worker worker = await db.Workers.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (worker == null)
                return NotFound();
            return new ObjectResult(worker);
        }

        // POST api/Workers
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<Worker>> Post([FromBody] Worker worker)
        {
            if (worker.Name == "admin")
            {
                ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Workers.Add(worker);
            await db.SaveChangesAsync();
            logger.LogInformation("Создан сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return worker;
        }

        // PUT api/Workers/
        [HttpPut]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<Worker>> Put(Worker worker)
        {
            if (worker == null)
            {
                return BadRequest();
            }
            if (!db.Workers.Any(x => x.Id == worker.Id))
            {
                return NotFound();
            }

            db.Update(worker);
            await db.SaveChangesAsync();
            logger.LogInformation("Отредактирован сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Ok(worker);
        }

        // DELETE api/Workers/5
        [HttpDelete("{id}")]
       // [Authorize(Roles = "admin")]
        public async Task<ActionResult<Worker>> Delete(string id)
        {
            Worker worker = db.Workers.FirstOrDefault(x => x.Id.ToString() == id);
            if (worker == null)
            {
                //return NotFound("Сотрудник с таким Id не найден");
                return NotFound(new { message = "Сотрудник с таким Id не найден" });
            }
            db.Workers.Remove(worker);
            await db.SaveChangesAsync();
            logger.LogInformation("Удален сотрудник {0} c id={1}, редактор {2}", worker.Name, worker.Id, User.Identity.Name);
            return Ok(worker);
        }
    }
}
