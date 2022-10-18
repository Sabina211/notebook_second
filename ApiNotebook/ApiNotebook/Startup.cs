using ApiNotebook.BusinessLogic;
using ApiNotebook.Data;
using ApiNotebook.Mappings;
using ApiNotebook.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNotebook
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiNotebook", Version = "v1" });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyApi.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<DataContext>()
                    .AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(AppMappingProfile));
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4; // минимальное количество знаков в пароле
                options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                options.Lockout.MaxFailedAccessAttempts = 10; // количество попыток до блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IWorkerRepository, WorkerRepository>();

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
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiNotebook v1"));
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
