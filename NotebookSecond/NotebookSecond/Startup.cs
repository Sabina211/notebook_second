using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotebookSecond.ContextFolder;
using NotebookSecond.Data;
using NotebookSecond.Entities;
using NotebookSecond.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHttpClient("httpClient", c => c.BaseAddress = new System.Uri("https://localhost:5005"));
            services.AddTransient<IWorkerData, ApiWorkerData>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4; // ����������� ���������� ������ � ������
                options.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                options.Lockout.MaxFailedAccessAttempts = 10; // ���������� ������� �� ����������
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // ������������ Cookie � ����� ������������� �� ��� �������� �����������
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseRouting();

            app.UseAuthentication();    // ����������� ��������������
            app.UseAuthorization();
            app.UseMvc(
                r =>
                {
                    r.MapRoute("default", "{controller=Worker}/{action=Index}");
                });
        }
    }
}
