using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotebookSecond.Data;
using System;
using System.Threading.Tasks;

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
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                        options.Cookie.MaxAge = options.ExpireTimeSpan; // optional
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                        options.Events.OnSigningIn = (context) => { 
                            context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(30);
                            return Task.CompletedTask;
                        };
                    });
            services.AddAuthorization();
            services.AddHttpClient("httpClient", c => c.BaseAddress = new System.Uri("http://notebook-api/api/")).SetHandlerLifetime(TimeSpan.FromMinutes(30)) ;
            services.AddTransient<IWorkerData, ApiWorkerData>();

            services.AddMvc(options => options.EnableEndpointRouting = false) ;
            services.AddControllersWithViews();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4; // минимальное количество знаков в пароле
                options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                options.Lockout.MaxFailedAccessAttempts = 10; // количество попыток до блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
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
