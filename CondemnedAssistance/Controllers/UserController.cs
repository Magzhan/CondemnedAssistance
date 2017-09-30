using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers
{
    [Authorize(Roles = "2, 3")]
    public class UserController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private IAuthorizationService _authorizationService;
        private RegisterHelper registerHelper;
        private LinkHelper linkHelper;

        public UserController(UserContext context, IAuthorizationService authorizationService) {
            this._db = context;
            this._authorizationService = authorizationService;
            this.registerHelper = new RegisterHelper(context);
            this.linkHelper = new LinkHelper(context, "userEdit");
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<User> users = new List<User>();
            ICollection<UserProfileModel> model = new List<UserProfileModel>();
            if (User.IsInRole("3")) {
                users = _db.Users.ToList();
            }
            else {
                int[] usersWithAllowedRoles = _db.UserRoles.Where(r => r.RoleId != 3).Select(r => r.UserId).ToArray();
                int currUserRegisterId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);
                int[] registers = registerHelper.GetRegisterChildren(new int[] { }, currUserRegisterId);
                List<int> tempRegisters = new List<int> { currUserRegisterId };
                tempRegisters.AddRange(registers);
                registers = tempRegisters.ToArray();
                int[] usersWithAllowedRegisters = _db.UserRegisters.Where(r => registers.Contains(r.RegisterId)).Select(r => r.UserId).ToArray();

                int[] allowedUsersList = usersWithAllowedRegisters.Intersect(usersWithAllowedRoles).ToArray();

                users = _db.Users.Where(u => allowedUsersList.Contains(u.Id)).ToList();

                //users = (from u in _db.Users
                //         join ur in _db.UserRoles on u.Id equals ur.UserId into j
                //         from user in j.DefaultIfEmpty()
                //         where user.RoleId != 3 || user == null
                //         orderby u.Id descending
                //         select u).ToList();
            }
            foreach (User user in users) {
                UserStaticInfo info = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == user.Id);
                Role role = _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId);
                Register register = null;
                UserStatus userStatus = null;
                if (info != null) {
                    userStatus = _db.UserStatuses.FirstOrDefault(s => s.Id == info.UserStatusId);
                }
                UserRegister userRegister = _db.UserRegisters.FirstOrDefault(u => u.UserId == user.Id);
                if(userRegister != null) {
                    register = _db.Registers.FirstOrDefault(r => r.Id == userRegister.RegisterId);
                }
                UserProfileModel userProf = new UserProfileModel {
                    UserId = user.Id,
                    Login = user.Login,
                    LastName = info?.LastName,
                    FirstName = info?.FirstName,
                    MiddleName = info?.MiddleName,
                    Role = role?.Name,
                    Status = userStatus?.Name,
                    Registration = register?.Name
                };

                model.Add(userProf);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult View(int id) {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            UserStaticInfo userStaticInfo = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == id);
            UserRole myRole = _db.UserRoles.FirstOrDefault(u => u.UserId == id);
            UserStatus userStatus = _db.UserStatuses.FirstOrDefault(u => u.Id == userStaticInfo.UserStatusId);
            UserType userType = _db.UserTypes.FirstOrDefault(u => u.Id == userStaticInfo.UserTypeId);
            Role role = _db.Roles.FirstOrDefault(r => r.Id == myRole.RoleId);

            if (user == null) {
                return RedirectToAction("Index", "User");
            }

            UserModel model = new UserModel {
                UserId = id,
                Login = user.Login,
                LastName = userStaticInfo.LastName,
                FirstName = userStaticInfo.FirstName,
                MiddleName = userStaticInfo.MiddleName,
                Xin = userStaticInfo.Xin,
                Birthdate = userStaticInfo.Birthdate,
                Gender = userStaticInfo.Gender,
                UserStatusId = userStatus.Id,
                UserStatusName = userStatus.Name,
                UserTypeId = userType.Id,
                UserTypeName = userType.Name,
                RoleId = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create() {
            UserModelCreate model = new UserModelCreate();

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User,
                PersistenceHelperMode.Read, _db, PersistenceState.Create, model);

            userPersistenceHelper.LoadModel();

            model = userPersistenceHelper.GetModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModelCreate model) {
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "roleId", model.RoleId },
                { "childId", model.UserRegisterId }
            };

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User, 
                PersistenceHelperMode.Write, _db, PersistenceState.Create, model);

            if (ModelState.IsValid) {
                if (userPersistenceHelper.Validate(out string message)) {

                    userPersistenceHelper.Persist(out message);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Login", message);
                userPersistenceHelper.LoadModel();
                model = userPersistenceHelper.GetModel();
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id) {

            Dictionary<string, int> actions = new Dictionary<string, int>();
            if(_db.UserRoles.Any(u => u.UserId == id)) {
                actions.Add("roleId", _db.UserRoles.First(u => u.UserId == id).RoleId);
            }
            if (_db.UserRegisters.Any(u => u.UserId == id)) {
                actions.Add("childId", _db.UserRegisters.Single(u => u.UserId == id).RegisterId);
            }
            int operatorRegisterId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User,
                PersistenceHelperMode.Read, _db, PersistenceState.Update, new UserModelCreate { UserId = id });

            userPersistenceHelper.LoadModel();
            UserModelCreate model = userPersistenceHelper.GetModel();

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", id.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = true,
                Text = "Персональные данные",
                RouteValues = routeVals
            });

            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", id.ToString());

            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = false,
                Text = "История",
                RouteValues = routeVals1
            });

            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", id.ToString()); 
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = false,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UserModelCreate model) {

            Dictionary<string, int> actions = new Dictionary<string, int>();
            if (_db.UserRoles.Any(u => u.UserId == id)) {
                actions.Add("roleId", _db.UserRoles.First(u => u.UserId == id).RoleId);
            }
            actions.Add("childId", model.UserRegisterId);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded)  return new ChallengeResult(); 

            if (_db.UserRegisters.Any(u => u.UserId == id)) actions["childId"] = _db.UserRegisters.Single(u => u.UserId == id).RegisterId; 

            authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) return new ChallengeResult();

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User,
                PersistenceHelperMode.Write, _db, PersistenceState.Update, model);

            if (ModelState.IsValid) {
                if (userPersistenceHelper.Persist(out string message)) {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", message);
            }
            userPersistenceHelper.LoadModel();
            model = userPersistenceHelper.GetModel();

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", id.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = true,
                Text = "Персональные данные",
                RouteValues = routeVals
            });

            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", id.ToString());

            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = false,
                Text = "История",
                RouteValues = routeVals1
            });

            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", id.ToString()); 
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = false,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> History(int userId) {
            Dictionary<string, int> actions = new Dictionary<string, int>();
            if (_db.UserRoles.Any(u => u.UserId == userId))  actions.Add("roleId", _db.UserRoles.First(u => u.UserId == userId).RoleId); 

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (_db.UserRegisters.Any(u => u.UserId == userId)) actions["childId"] = _db.UserRegisters.Single(u => u.UserId == userId).RegisterId;

            authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) return new ChallengeResult();

            List<UserHistoryModel> model = _db.UserHistory.Where(h => h.Id == userId).Select(h => new UserHistoryModel { UserId = h.Id, OperationDate = h.RequestDate, OperatorFullName = _db.UserStaticInfo.Single(u => u.UserId == h.RequestUser).FirstName, TransactionId = h.TransactionId }).OrderByDescending(h => h.TransactionId).ToList();

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = false,
                Text = "Персональные данные",
                RouteValues = routeVals
            });
            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());
            links.Add(new LinkClass  {
                Controller = "User",
                Action = "History",
                IsSelected = true,
                Text = "История",
                RouteValues = routeVals1
            });

            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = false,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> HistoryDetail(long transactionId, int userId) {
            Dictionary<string, int> actions = new Dictionary<string, int>();
            if (_db.UserRoles.Any(u => u.UserId == userId)) actions.Add("roleId", _db.UserRoles.First(u => u.UserId == userId).RoleId);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (_db.UserRegisters.Any(u => u.UserId == userId)) actions["childId"] = _db.UserRegisters.Single(u => u.UserId == userId).RegisterId;

            authResult = await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) return new ChallengeResult();



            return View();
        }
    }
}