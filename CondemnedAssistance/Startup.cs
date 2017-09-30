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
using CondemnedAssistance.Services.WebSockets;
using CondemnedAssistance.Hubs;
using CondemnedAssistance.Services.Sms;
using CondemnedAssistance.Services.IService;
using CondemnedAssistance.Services.Email;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Address;
using CondemnedAssistance.Services.Security.RoleAuthorization;
using System.Linq;

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

            services.AddAuthentication("Cookies")
                .AddCookie(options => {
                    options.AccessDeniedPath = "/Home/Error";
                    options.LoginPath = "/Account/Login";
                });

            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            services.AddMvc(options => {
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthorization(options => {
                options.AddPolicy("resource-register-actions-policy", x => { x.AddRequirements(new ResourceRegisterBasedRequirement()); });
            });

            services.AddSingleton<IAuthorizationHandler, ResourceRegisterHandler>();

            services.AddWebSocketManager();

            services.AddTransient<IMessageSender, SmsSender>();
            //services.AddTransient<IMessageSender, EmailSender>();

            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) {
            // Order
            //Exception / error handling
            //Static file server
            //Authentication
            //Websockets
            //MVC

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/error");
            }
            app.UseWebSockets();
            app.UseStaticFiles();

            app.UseAuthentication();

            loggerFactory.AddConsole();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<UserContext>();
                context.Database.Migrate();
            }

            app.ApplicationServices.GetRequiredService<UserContext>().SeedAsync();

            app.UseMvc(routes => {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapWebSocketManager("/messageshub", serviceProvider.GetService<WebSocketMessageHandler>());
        }
    }

    public static class DbContextExtensions {
        public static async void SeedAsync(this UserContext context) {
            await AddData(context);
        }

        private static async Task AddData(UserContext context) {
            if(!await context.Controllers.AnyAsync()) {
                await context.Controllers.AddRangeAsync(
                    new Controller { Name = Constants.Address, NormalizedName = Constants.Address.ToUpper(), Description = "Address Controller", RequestDate = DateTime.Now, RequestUser = -1 }
                );

                await context.SaveChangesAsync();
            }

            if(!await context.Actions.AnyAsync()) {
                int controllerId =  context.Controllers.Single(c => c.NormalizedName == Constants.Address.ToUpper()).Id;
                await context.Actions.AddRangeAsync(
                    new Models.Action { Name = AddressOperations.Create.Name, NormalizedName = AddressOperations.Create.Name, Description = "Address Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                    new Models.Action { Name = AddressOperations.Read.Name, NormalizedName = AddressOperations.Read.Name, Description = "Address Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                    new Models.Action { Name = AddressOperations.Update.Name, NormalizedName = AddressOperations.Update.Name, Description = "Address Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                    new Models.Action { Name = AddressOperations.Delete.Name, NormalizedName = AddressOperations.Delete.Name, Description = "Address Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
                );

                await context.SaveChangesAsync();
            }

            if(!await context.RoleAccesses.AnyAsync()) {
                int controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Address.ToUpper()).Id;
                int actionId = context.Actions.Single(a => a.NormalizedName == AddressOperations.Create.Name & a.ControllerId == controllerId).Id;
                int roleId = 3;
                await context.RoleAccesses.AddRangeAsync(
                    new RoleAccess { RoleId = roleId, ControllerId = controllerId, ActionId = actionId, RequestDate = DateTime.Now, RequestUser = -1}
                );
            }

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
