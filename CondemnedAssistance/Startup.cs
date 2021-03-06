﻿using CondemnedAssistance.Models;
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
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Address;
using CondemnedAssistance.Services.Security.RoleAuthorization;
using System.Linq;
using CondemnedAssistance.Services.Security.Event;
using CondemnedAssistance.Services.Security.Help;
using CondemnedAssistance.Services.Security.Message;
using CondemnedAssistance.Services.Security.Profession;
using CondemnedAssistance.Services.Security.Register;
using CondemnedAssistance.Services.Security.Role;
using CondemnedAssistance.Services.Security.User;
using System.Collections.Generic;

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
            string connectionStringApp = Configuration.GetConnectionString("ApplicationConnection");

            services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionStringApp));

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

            app.UseWebSockets();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/error");
            }
            app.UseStaticFiles();

            app.UseAuthentication();

            loggerFactory.AddConsole();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<UserContext>();
                context.Database.Migrate();

                var context2 = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                context2.Database.Migrate();
            }

            app.ApplicationServices.GetRequiredService<ApplicationContext>().SeedAsyncApp();
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

        public static async void SeedAsyncApp(this ApplicationContext context) {
            await AddData(context);
        }

        private static async Task AddData(ApplicationContext context) {
            List<Controller> controllers = new List<Controller> {
                new Controller { Name = Constants.Address, NormalizedName = Constants.Address.ToUpper(), Description = "Address Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Education, NormalizedName = Constants.Education.ToUpper(), Description = "Education Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Email, NormalizedName = Constants.Email.ToUpper(), Description = "Email Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Event, NormalizedName = Constants.Event.ToUpper(), Description = "Event Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Help, NormalizedName = Constants.Help.ToUpper(), Description = "Help Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Message, NormalizedName = Constants.Message.ToUpper(), Description = "Message Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Profession, NormalizedName = Constants.Profession.ToUpper(), Description = "Profession Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Register, NormalizedName = Constants.Register.ToUpper(), Description = "Register Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Role, NormalizedName = Constants.Role.ToUpper(), Description = "Role Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Sms, NormalizedName = Constants.Sms.ToUpper(), Description = "Sms Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.User, NormalizedName = Constants.User.ToUpper(), Description = "User Controller", RequestDate = DateTime.Now, RequestUser = -1 },
                new Controller { Name = Constants.Vacancy, NormalizedName = Constants.Vacancy.ToUpper(), Description = "Vacancy Controller", RequestDate = DateTime.Now, RequestUser = -1 }
            };

            if(!await context.Controllers.AnyAsync()) {
                await context.Controllers.AddRangeAsync(controllers);

                await context.SaveChangesAsync();
            }
            int controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Address.ToUpper()).Id;
            List<Models.Action> AddressActions = new List<Models.Action> {
                new Models.Action { Name = AddressOperations.Create.Name, NormalizedName = AddressOperations.Create.Name, Description = "Address Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.Read.Name, NormalizedName = AddressOperations.Read.Name, Description = "Address Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.Update.Name, NormalizedName = AddressOperations.Update.Name, Description = "Address Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.Delete.Name, NormalizedName = AddressOperations.Delete.Name, Description = "Address Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.AddressLevels.Name, NormalizedName = AddressOperations.AddressLevels.Name, Description = "Address Controller, AddressLevels Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.CreateLevel.Name, NormalizedName = AddressOperations.CreateLevel.Name, Description = "Address Controller, CreateLevel Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.UpdateLevel.Name, NormalizedName = AddressOperations.UpdateLevel.Name, Description = "Address Controller, UpdateLevel Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.DeleteLevel.Name, NormalizedName = AddressOperations.DeleteLevel.Name, Description = "Address Controller, DeleteLevel Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = AddressOperations.GetAddressList.Name, NormalizedName = AddressOperations.GetAddressList.Name, Description = "Address Controller, GetAddressList Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Event.ToUpper()).Id;
            List<Models.Action> EventActions = new List<Models.Action> {
                new Models.Action { Name = EventOperations.Create.Name, NormalizedName = EventOperations.Create.Name, Description = "Event Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1},
                new Models.Action { Name = EventOperations.Read.Name, NormalizedName = EventOperations.Read.Name, Description = "Event Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = EventOperations.Update.Name, NormalizedName = EventOperations.Update.Name, Description = "Event Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = EventOperations.Delete.Name, NormalizedName = EventOperations.Delete.Name, Description = "Event Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = EventOperations.EventStatuses.Name, NormalizedName = EventOperations.EventStatuses.Name, Description = "Event Controller, EventStatuses Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = EventOperations.CreateStatus.Name, NormalizedName = EventOperations.CreateStatus.Name, Description = "Event Controller, CreateStatus Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = EventOperations.UpdateStatus.Name, NormalizedName = EventOperations.UpdateStatus.Name, Description = "Event Controller, UpdateStatus Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = EventOperations.DeleteStatus.Name, NormalizedName = EventOperations.DeleteStatus.Name, Description = "Event Controller, DeleteStatus Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Help.ToUpper()).Id;
            List<Models.Action> HelpActions = new List<Models.Action> {
                new Models.Action { Name = HelpOperations.Read.Name, NormalizedName = HelpOperations.Read.Name, Description = "Help Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = HelpOperations.Create.Name, NormalizedName = HelpOperations.Create.Name, Description = "Help Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = HelpOperations.Update.Name, NormalizedName = HelpOperations.Update.Name, Description = "Help Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = HelpOperations.Delete.Name, NormalizedName = HelpOperations.Delete.Name, Description = "Help Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = HelpOperations.AddUserHelp.Name, NormalizedName = HelpOperations.AddUserHelp.Name, Description = "Help Controller, AddUserHelp Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = HelpOperations.UserHelpList.Name, NormalizedName = HelpOperations.UserHelpList.Name, Description = "Help Controller, UserHelpList Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Message.ToUpper()).Id;
            List<Models.Action> MessageActions = new List<Models.Action> {
                new Models.Action { Name = MessageOperations.Read.Name, NormalizedName = MessageOperations.Read.Name, Description = "Message Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = MessageOperations.LoadUsers.Name, NormalizedName = MessageOperations.LoadUsers.Name, Description = "Message Controller, LoadUsers Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = MessageOperations.LoadMessages.Name, NormalizedName = MessageOperations.LoadMessages.Name, Description = "Message Controller, LoadMessages Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = MessageOperations.Send.Name, NormalizedName = MessageOperations.Send.Name, Description = "Message Controller, Send Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Profession.ToUpper()).Id;
            List<Models.Action> ProfessionActions = new List<Models.Action> {
                new Models.Action { Name = ProfessionOperations.Read.Name, NormalizedName = ProfessionOperations.Read.Name, Description = "Profession Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = ProfessionOperations.Create.Name, NormalizedName = ProfessionOperations.Create.Name, Description = "Profession Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = ProfessionOperations.Update.Name, NormalizedName = ProfessionOperations.Update.Name, Description = "Profession Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = ProfessionOperations.Delete.Name, NormalizedName = ProfessionOperations.Delete.Name, Description = "Profession Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Register.ToUpper()).Id;
            List<Models.Action> RegisterActions = new List<Models.Action> {
                new Models.Action { Name = RegisterOperations.Create.Name, NormalizedName = RegisterOperations.Create.Name, Description = "Register Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.Read.Name, NormalizedName = RegisterOperations.Read.Name, Description = "Register Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.Update.Name, NormalizedName = RegisterOperations.Update.Name, Description = "Register Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.Delete.Name, NormalizedName = RegisterOperations.Delete.Name, Description = "Register Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.RegisterLevels.Name, NormalizedName = RegisterOperations.RegisterLevels.Name, Description = "Register Controller, RegisterLevels Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.CreateLevel.Name, NormalizedName = RegisterOperations.CreateLevel.Name, Description = "Register Controller, CreateLevel Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.UpdateLevel.Name, NormalizedName = RegisterOperations.UpdateLevel.Name, Description = "Register Controller, UpdateLevel Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RegisterOperations.DeleteLevel.Name, NormalizedName = RegisterOperations.DeleteLevel.Name, Description = "Register Controller, DeleteLevel Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.Role.ToUpper()).Id;
            List<Models.Action> RoleActions = new List<Models.Action> {
                new Models.Action { Name = RoleOperations.Read.Name, NormalizedName = RoleOperations.Read.Name, Description = "Role Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RoleOperations.Create.Name, NormalizedName = RoleOperations.Create.Name, Description = "Role Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RoleOperations.Update.Name, NormalizedName = RoleOperations.Update.Name, Description = "Role Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RoleOperations.Delete.Name, NormalizedName = RoleOperations.Delete.Name, Description = "Role Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = RoleOperations.RoleAccess.Name, NormalizedName = RoleOperations.RoleAccess.Name, Description = "Role Controller, RoleAccess Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1}
            };

            controllerId = context.Controllers.Single(c => c.NormalizedName == Constants.User.ToUpper()).Id;
            List<Models.Action> UserActions = new List<Models.Action> {
                new Models.Action { Name = UserOperations.Read.Name, NormalizedName = UserOperations.Read.Name, Description = "User Controller, Read Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = UserOperations.Create.Name, NormalizedName = UserOperations.Create.Name, Description = "User Controller, Create Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = UserOperations.Update.Name, NormalizedName = UserOperations.Update.Name, Description = "User Controller, Update Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = UserOperations.Delete.Name, NormalizedName = UserOperations.Delete.Name, Description = "User Controller, Delete Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = UserOperations.History.Name, NormalizedName = UserOperations.History.Name, Description = "User Controller, History Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 },
                new Models.Action { Name = UserOperations.HistoryDetail.Name, NormalizedName = UserOperations.HistoryDetail.Name, Description = "User Controller, HistoryDetail Action", ControllerId = controllerId, RequestDate = DateTime.Now, RequestUser = -1 }
            };
            if (!await context.Actions.AnyAsync()) {
                
                await context.Actions.AddRangeAsync(AddressActions);
                await context.Actions.AddRangeAsync(EventActions);
                await context.Actions.AddRangeAsync(HelpActions);
                await context.Actions.AddRangeAsync(MessageActions);
                await context.Actions.AddRangeAsync(ProfessionActions);
                await context.Actions.AddRangeAsync(RegisterActions);
                await context.Actions.AddRangeAsync(RoleActions);
                await context.Actions.AddRangeAsync(UserActions);

                await context.SaveChangesAsync();
            }

            List<Models.Action> actions = new List<Models.Action>();
            actions.AddRange(AddressActions);
            actions.AddRange(EventActions);
            actions.AddRange(HelpActions);
            actions.AddRange(MessageActions);
            actions.AddRange(ProfessionActions);
            actions.AddRange(RegisterActions);
            actions.AddRange(RoleActions);
            actions.AddRange(UserActions);

            foreach(Controller c in controllers) {
                if(!await context.Controllers.AnyAsync(ct => ct.NormalizedName == c.NormalizedName)) {
                    await context.Controllers.AddAsync(c);
                }
            }
            await context.SaveChangesAsync();

            foreach(Models.Action a in actions) {
                if(!await context.Actions.AnyAsync(ac => ac.NormalizedName == a.NormalizedName)) {
                    await context.Actions.AddAsync(a);
                }
            }
            await context.SaveChangesAsync();

            int roleId = 3;
            foreach(Controller c in context.Controllers.ToList()) {
                foreach(Models.Action a in context.Actions.Where(action => action.ControllerId == c.Id).ToList()) {
                    if(!await context.RoleAccesses.AnyAsync(r => r.ControllerId == c.Id & r.ActionId == a.Id & r.RoleId == roleId))
                    await context.RoleAccesses.AddAsync(
                        new RoleAccess { RoleId = roleId, ControllerId = c.Id, ActionId = a.Id, RequestDate = DateTime.Now, RequestUser = -1, IsAllowed = true}
                    );
                }
            }
            await context.SaveChangesAsync();
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

            if (!await context.Types.AnyAsync()) {
                await context.Types.AddAsync(new Models.Type { Name = "Пользователь", NormalizedName = "Пользователь".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1});
                await context.SaveChangesAsync();
            }

            var resp = new RegisterLevel { Name = "Республика", IsFirstAncestor = true, IsLastChild = false, NormalizedName = "Республика".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1 };
            var obl = new RegisterLevel { Name = "Облыс", IsFirstAncestor = false, IsLastChild = false, NormalizedName = "Облыс".ToUpper(), Description = "2", RequestDate = DateTime.Now, RequestUser = -1 };
            var reg = new RegisterLevel { Name = "Район", IsFirstAncestor = false, IsLastChild = true, NormalizedName = "Район".ToUpper(), Description = "3", RequestDate = DateTime.Now, RequestUser = -1 };

            if (!await context.RegisterLevels.AnyAsync()) {
                await context.RegisterLevels.AddRangeAsync(resp, obl, reg);
                await context.SaveChangesAsync();
            }

            if(!await context.RegisterLevelHierarchies.AnyAsync()) {
                await context.RegisterLevelHierarchies.AddRangeAsync(
                    new RegisterLevelHierarchy {  ParentLevel = resp.Id, ChildLevel = obl.Id },
                    new RegisterLevelHierarchy {  ParentLevel = obl.Id, ChildLevel = reg.Id }
                );
            }

            if (!await context.Registers.AnyAsync()) {
                await context.Registers.AddAsync(
                    new Register { Name = "Қазақстан", NormalizedName = "Қазақстан".ToUpper(), Description = "1", RegisterLevelId = 1, RequestDate = DateTime.Now, RequestUser = -1 }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Statuses.AnyAsync()) {
                await context.Statuses.AddAsync(new Status { Name = "Активный", NormalizedName = "Активный".ToUpper(), Description = "1", RequestDate = DateTime.Now, RequestUser = -1 });
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

            if (!await context.Users.AnyAsync()) {
                User system = new User {
                    Email = "test@probaciya.kz",
                    EmailConfirmed = false,
                    Login = "000000000000",
                    PasswordHash = "qwerty",
                    NormalizedEmail = "test@probaciya.kz".ToUpper(),
                    PhoneNumber = "000000",
                    PhoneNumberConfirmed = false,
                    AccessFailedCount = 0,
                    RequestDate = DateTime.Now,
                    RequestUser = -1
                };
                User admin1 = new User {
                    Email = "magzhan_alter@mail.ru",
                    EmailConfirmed = false,
                    Login = "931023350276",
                    PasswordHash = "qwerty",
                    NormalizedEmail = "magzhan_alter@mail.ru".ToUpper(),
                    PhoneNumber = "87077524956",
                    PhoneNumberConfirmed = false,
                    AccessFailedCount = 0,
                    RequestDate = DateTime.Now,
                    RequestUser = -1
                };

                User admin2 = new User
                {
                    Email = "akylzhan.bidakhmetov@gmail.com",
                    EmailConfirmed = false,
                    Login = "930615350539",
                    PasswordHash = "qwerty",
                    NormalizedEmail = "akylzhan.bidakhmetov@gmail.com".ToUpper(),
                    PhoneNumber = "87019677200",
                    PhoneNumberConfirmed = false,
                    AccessFailedCount = 0,
                    RequestDate = DateTime.Now,
                    RequestUser = -1
                };
                UserStaticInfo admin1info = new UserStaticInfo {
                    Birthdate = new DateTime(1993, 10, 23),
                    FirstName = "Magzhan",
                    Gender = true, // Male
                    LastName = "Yelshibayev",
                    MainAddress = "Uly Dala 29",
                    MiddleName = "Kayratuli",
                    RequestDate = DateTime.Now,
                    RequestUser = -1,
                    Xin = "931023350276",
                    UserStatusId = 1,
                    UserTypeId = 1
                };
                UserStaticInfo admin2info = new UserStaticInfo
                {
                    Birthdate = new DateTime(1993, 06, 15),
                    FirstName = "Akylzhan  ",
                    Gender = true, // Male
                    LastName = "Bidakhmetov",
                    MainAddress = "Gete 107",
                    MiddleName = "Nurlanuly",
                    RequestDate = DateTime.Now,
                    RequestUser = -1,
                    Xin = "930615350539",
                    UserStatusId = 1,
                    UserTypeId = 1
                };
                UserRole admin1role = new UserRole {
                    RoleId = 3,
                    UserId = 2,
                    RequestDate = DateTime.Now,
                    RequestUser = -1
                };
                UserRole admin2role = new UserRole
                {
                    RoleId = 3,
                    UserId = 3,
                    RequestDate = DateTime.Now,
                    RequestUser = -1
                };
                UserRegister admin1register = new UserRegister {
                    UserId = 2,
                    RegisterId = 1,
                    RequestUser = -1,
                    RequestDate = DateTime.Now
                };
                UserRegister admin2register = new UserRegister
                {
                    UserId = 3,
                    RegisterId = 1,
                    RequestUser = -1,
                    RequestDate = DateTime.Now
                };
                await context.Users.AddRangeAsync(
                    system, admin1,admin2
                    );
                await context.SaveChangesAsync();

                admin1info.UserId = admin1.Id;
                admin2info.UserId = admin2.Id;
                await context.UserInfo.AddRangeAsync(admin1info, admin2info);
                admin1role.UserId = admin1.Id;
                admin2role.UserId = admin2.Id;
                await context.UserRoles.AddRangeAsync(admin1role, admin2role);
                admin1register.UserId = admin1.Id;
                admin2register.UserId = admin2.Id;
                await context.UserRegisters.AddRangeAsync(admin1register, admin2register);
            }
            await context.SaveChangesAsync();
        }
    }
}
