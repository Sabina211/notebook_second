using Microsoft.EntityFrameworkCore;
using NotebookSecond.ContextFolder;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            if (context.Workers.Any()) return;

            var sections = new List<Worker>()
            {
                new Worker(){Id = Guid.NewGuid(), Name="Елена", Surname="Еленова" },
                new Worker(){Id = Guid.NewGuid(), Surname="Марков", Name="Марк"},
                new Worker(){Id = Guid.NewGuid(), Surname="Карлов", Name="Карл" }
            };
            using (var trans = context.Database.BeginTransaction())
            {
                foreach (var section in sections)
                {
                    context.Workers.Add(section);
                }

               // context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Workers] ON");
                context.SaveChanges();
                //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Workers] OFF");
                trans.Commit();
            }


        }
    }
}
