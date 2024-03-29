﻿using ApiNotebook.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook.Data
{
    // Seeder
    // Seed data
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();
            if (context.Workers.Any()) return;

            var sections = new List<Worker>()
            {
                new Worker(){Id = Guid.NewGuid(), Name="Елена", Surname="Еленова" },
                new Worker(){Id = Guid.NewGuid(), Surname="Марков", Name="Марк", Patronymic="Маркович"},
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


        public static async Task InitializeRoles(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminLogin = "admin";
            string userLogin = "Karl";
            string password = "Qwerty123";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User { UserName = adminLogin };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (await userManager.FindByNameAsync(userLogin) == null)
            {
                User user = new User { UserName = userLogin };
                IdentityResult result = await userManager.CreateAsync(user, password);
            }
        }
    }
}
