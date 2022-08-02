using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotebookSecond.ContextFolder;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Web;
using NLog;

namespace NotebookSecond
{
    public class Program
    {
        
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            try
            {
                logger.Debug("init main2");
                //var init = BuildWebHost(args);
                var init = BuildWebHost(args);
                using (var scope = init.Services.CreateScope())
                {
                    var s = scope.ServiceProvider;
                    var c = s.GetRequiredService<DataContext>();
                    DbInitializer.Initialize(c);

                    try
                    {
                        var userManager = s.GetRequiredService<UserManager<User>>();
                        var rolesManager = s.GetRequiredService<RoleManager<IdentityRole>>();
                        await DbInitializer.InitializeRoles(userManager, rolesManager);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occurred while seeding the database.");
                    }
                }
                logger.Debug("перед ран");
                init.Run();

            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                logger.Debug("закрытие");
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
             .ConfigureLogging(logging =>
             {
                 logging.ClearProviders();
                 logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                 //logging.AddNLog(config);
             })
              .UseNLog();  

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();


    }
}

