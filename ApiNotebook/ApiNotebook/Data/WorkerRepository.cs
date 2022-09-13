using ApiNotebook.Models;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly DataContext _db;

        public WorkerRepository(DataContext db)
        {
            _db = db;
        }

        public async Task Add(Worker worker)
        {
            _db.Workers.Add(worker);
            await _db.SaveChangesAsync();
        }
    }
}
