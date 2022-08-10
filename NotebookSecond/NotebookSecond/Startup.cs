using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotebookSecond.Data;
using System;

namespace NotebookSecond
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles();
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme, (options) =>
                    {
                        options.Cookie.HttpOnly = true;
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                    });
            services.AddAuthorization();
            services.AddHttpClient("httpClient", c => c.BaseAddress = new System.Uri("https://localhost:5005"));
            services.AddTransient<IWorkerData, ApiWorkerData>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllersWithViews();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4; // минимальное количество знаков в пароле
                options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                options.Lockout.MaxFailedAccessAttempts = 10; // количество попыток до блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();    
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseMvc(
                r =>
                {
                    r.MapRoute("default", "{controller=Worker}/{action=Index}");
                });
        }
    }
}
