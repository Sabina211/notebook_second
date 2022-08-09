using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotebookSecond.ContextFolder;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using System;
using System.Threading.Tasks;
using NLog.Web;
using NLog;

namespace NotebookSecond
{
    public class Program
    {
        
        public static async Task Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            try
            {

                logger.Info("init main2");
                var init = CreateHostBuilder(args).Build();
                using (var scope = init.Services.CreateScope())
                {
                    var s = scope.ServiceProvider;
                    //var c = s.GetRequiredService<DataContext>();
                    //DbInitializer.Initialize(c);

                    try
                    {
                        /*var userManager = s.GetRequiredService<UserManager<User>>();
                        var rolesManager = s.GetRequiredService<RoleManager<IdentityRole>>();
                        await DbInitializer.InitializeRoles(userManager, rolesManager);*/
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occurred while seeding the database.");
                    }
                }
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
                LogManager.Shutdown();
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
             })
              .UseNLog();  
    }
}

