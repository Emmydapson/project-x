using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using LearningStarter.Data;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningStarter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.MaxValue;
                options.Preload = true;
                options.IncludeSubDomains = true;
            });

            // Configure the database context to use SQL Server
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Set up Identity
            services.AddIdentity<User, Role>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                    options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                    options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
                })
                .AddEntityFrameworkStores<DataContext>();

            services.AddMvc();

            // Cookie Authentication setup
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization();

            // Swagger setup for API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Learning Starter Server",
                    Version = "v1",
                    Description = "Description for the API goes here.",
                });

                c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
                c.MapType(typeof(IFormFile), () => new OpenApiSchema { Type = "file", Format = "binary" });
            });

            // Setup for SPA static files (if applicable)
            services.AddSpaStaticFiles(config => { config.RootPath = "learning-starter-web/build"; });

            services.AddHttpContextAccessor();

            // Dependency Injection setup for application services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Add logging to capture startup issues
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            // Apply migrations to ensure database is up to date (instead of EnsureCreated)
            dataContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show detailed error pages in development
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Handle errors in production
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles(); // Serve SPA static files

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Global CORS policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Enable Swagger for API documentation
            app.UseSwagger(options => { options.SerializeAsV2 = true; });

            // Enable Swagger UI for browsing API
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning Starter Server API V1"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Serve SPA if in development mode
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "learning-starter-web"; // Adjust to your SPA source path

                if (env.IsDevelopment())
                {
                    // Proxy requests to your SPA development server
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3001");
                }
            });

            // Seed the database with roles, users, and server types
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

            SeedRoles(dataContext, roleManager).Wait();
            SeedUsers(dataContext, userManager).Wait();
            SeedServerTypes(dataContext);
        }

        // Seed roles in the database
        private static async Task SeedRoles(DataContext dataContext, RoleManager<Role> roleManager)
        {
            if (dataContext.Roles.Any()) return;

            var seededRole = new Role { Name = "Admin" };
            await roleManager.CreateAsync(seededRole);
            await dataContext.SaveChangesAsync();
        }

        // Seed users in the database
        private static async Task SeedUsers(DataContext dataContext, UserManager<User> userManager)
        {
            if (dataContext.Users.Any()) return;

            var seededUser = new User
            {
                FirstName = "Seeded",
                LastName = "User",
                UserName = "admin",
            };

            await userManager.CreateAsync(seededUser, "Password");
            await userManager.AddToRoleAsync(seededUser, "Admin");
            await dataContext.SaveChangesAsync();
        }

        // Seed server types in the database
        private static void SeedServerTypes(DataContext dataContext)
        {
            if (dataContext.Set<ServerTypes>().Any()) return;

            var serverTypes = new[]
            {
                new ServerTypes { Name = "School", Description = "" },
                new ServerTypes { Name = "Class", Description = "" }
            };

            dataContext.Set<ServerTypes>().AddRange(serverTypes);
            dataContext.SaveChanges();
        }
    }
}
