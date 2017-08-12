using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using CondemnedAssistance.Services.Requirements;
using CondemnedAssistance.Services.Resources;
using System.Threading.Tasks;
using System;

namespace CondemnedAssistance {
    public class Startup {

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services){
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            services.AddMvc(options => {
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthorization(options => {
                options.AddPolicy("resource-register-actions-policy", x => { x.AddRequirements(new ResourceRegisterBasedRequirement()); });
            });

            services.AddSingleton<IAuthorizationHandler, ResourceRegisterHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {

            app.UseStaticFiles();

            loggerFactory.AddConsole();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<UserContext>();
                context.Database.Migrate();
            }

            app.ApplicationServices.GetRequiredService<UserContext>().SeedAsync();

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationScheme = "Cookies",
                LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseDeveloperExceptionPage();
            //if (env.IsDevelopment()) {
                
            //}

            app.UseMvc(routes => {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class DbContextExtensions {
        public static async void SeedAsync(this UserContext context) {
            await AddData(context);
        }

        private static async Task AddData(UserContext context) {
            if (!await context.AddressLevels.AnyAsync()) {
                await context.AddressLevels.AddRangeAsync(
                    new AddressLevel { Name = "Республика", NormalizedName = "Республика".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1 },
                    new AddressLevel { Name = "Облыс", NormalizedName = "Облыс".ToUpper(), Description = "2", RequestDate = DateTime.Now, RequestUser = -1 },
                    new AddressLevel { Name = "Район", NormalizedName = "Район".ToUpper(), Description = "3", RequestDate = DateTime.Now, RequestUser = -1 }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Addresses.AnyAsync()) {
                await context.Addresses.AddAsync(
                    new Address { Name = "Қазақстан", NormalizedName = "Қазақстан".ToUpper(), Description = "1", AddressLevelId = 1, RequestDate = DateTime.Now, RequestUser = -1 }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.UserTypes.AnyAsync()) {
                await context.UserTypes.AddAsync(new UserType { Name = "Пользователь", NormalizedName = "Пользователь".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1});
                await context.SaveChangesAsync();
            }

            if (!await context.RegisterLevels.AnyAsync()) {
                await context.RegisterLevels.AddRangeAsync(
                    new RegisterLevel { Name = "Республика", NormalizedName = "Республика".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1},
                    new RegisterLevel { Name = "Облыс", NormalizedName = "Облыс".ToUpper(), Description = "2", RequestDate = DateTime.Now, RequestUser = -1 },
                    new RegisterLevel { Name = "Район", NormalizedName = "Район".ToUpper(), Description = "3", RequestDate = DateTime.Now, RequestUser = -1 }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Registers.AnyAsync()) {
                await context.Registers.AddAsync(
                    new Register { Name = "Қазақстан", NormalizedName = "Қазақстан".ToUpper(), Description = "1", RegisterLevelId = 1, RequestDate = DateTime.Now, RequestUser = -1 }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.UserStatuses.AnyAsync()) {
                await context.UserStatuses.AddAsync(new UserStatus { Name = "Активный", NormalizedName = "Активный".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1 });
                await context.SaveChangesAsync();
            }

            if (!await context.Roles.AnyAsync()) {
                await context.Roles.AddRangeAsync(
                    new Role { Name = "Пользователь", NormalizedName = "Пользователь".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1 },
                    new Role { Name = "Пробация", NormalizedName = "Пробация".ToUpper(), Description = "2", RequestDate = DateTime.Now, RequestUser = -1 },
                    new Role { Name = "Администратор", NormalizedName = "Администратор".ToUpper(), Description = "3", RequestDate = DateTime.Now, RequestUser = -1 }
                );
                await context.SaveChangesAsync();
            }
            await context.SaveChangesAsync();
        }
    }
}
