using Microsoft.EntityFrameworkCore;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.ContextFolder
{
    public class DataContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              @"Server=(localdb)\MSSQLLocalDB;
                               DataBase=_Notebook;
                               Trusted_Connection=True;"
              );
        }
    }
}
