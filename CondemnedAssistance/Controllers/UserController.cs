using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2, 3")]
    public class UserController : Controller {

        private UserContext _db;
        private IAuthorizationService _authorizationService;
        private RegisterHelper registerHelper;

        public UserController(UserContext context, IAuthorizationService authorizationService) {
            this._db = context;
            this._authorizationService = authorizationService;
            this.registerHelper = new RegisterHelper(context);
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<User> users = new List<User>();
            ICollection<UserProfileModel> model = new List<UserProfileModel>();
            if (User.IsInRole("3")) {
                users = _db.Users.ToList();
            }
            else {
                users = (from u in _db.Users
                         join ur in _db.UserRoles on u.Id equals ur.UserId into j
                         from user in j.DefaultIfEmpty()
                         where user.RoleId != 3 || user == null
                         orderby u.Id descending
                         select u).ToList();
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
                UserProfileModel userProf = new UserProfileModel();
                userProf.UserId = user.Id;
                userProf.Login = user.Login;
                userProf.LastName = info?.LastName;
                userProf.FirstName = info?.FirstName;
                userProf.MiddleName = info?.MiddleName;
                userProf.Role = role?.Name;
                userProf.Status = userStatus?.Name;
                userProf.Registration = register?.Name;

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

            UserModel model = new UserModel();

            model.UserId = id;
            model.Login = user.Login;
            model.LastName = userStaticInfo.LastName;
            model.FirstName = userStaticInfo.FirstName;
            model.MiddleName = userStaticInfo.MiddleName;
            model.Xin = userStaticInfo.Xin;
            model.Birthdate = userStaticInfo.Birthdate;
            model.Gender = userStaticInfo.Gender;
            model.UserStatusId = userStatus.Id;
            model.UserStatusName = userStatus.Name;
            model.UserTypeId = userType.Id;
            model.UserTypeName = userType.Name;
            model.RoleId = role.Id;
            model.RoleName = role.Name;

            return View(model);
        }

        [HttpGet]
        public IActionResult Create() {
            UserModelCreate model = new UserModelCreate();

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User,
                UserPersistenceHelperMode.Read, _db, UserPersistenceState.Create, model);

            model = userPersistenceHelper.GetModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModelCreate model) {
            Dictionary<string, int> actions = new Dictionary<string, int>();
            
            actions.Add("roleId", model.RoleId);
            actions.Add("childId", model.UserRegisterId);
            
            if(!await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User, 
                UserPersistenceHelperMode.Write, _db, UserPersistenceState.Create, model);

            if (ModelState.IsValid) {
                //if (!_db.Users.Any(u => u.Login == model.Login)) {
                string message;
                if (userPersistenceHelper.Validate(out message)) {

                    userPersistenceHelper.Persist(out message);
                    //User user = new User {
                    //    Login = model.Login,
                    //    Email = model.Email,
                    //    NormalizedEmail = model.Email.ToUpper(),
                    //    EmailConfirmed = false,
                    //    PhoneNumber = model.PhoneNumber,
                    //    PhoneNumberConfirmed = false,
                    //    AccessFailedCount = 0,
                    //    LockoutEnabled = false,
                    //    PasswordHash = "123456",
                    //    ModifiedUserDate = DateTime.Now,
                    //    ModifiedUserId = Convert.ToInt32(HttpContext.User.Identity.Name)
                    //};
                    //_db.Users.Add(user);
                    //_db.SaveChanges();

                    //UserStaticInfo userStaticInfo = new UserStaticInfo {
                    //    UserId = user.Id,
                    //    LastName = model.LastName,
                    //    FirstName = model.FirstName,
                    //    MiddleName = model.MiddleName,
                    //    Birthdate = model.Birthdate,
                    //    Gender = model.Gender,
                    //    Xin = model.Login,
                    //    MainAddress = model.MainAddress,
                    //    RequestDate = DateTime.Now,
                    //    RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                    //    UserTypeId = model.UserTypeId,
                    //    UserStatusId = model.UserStatusId
                    //};

                    //_db.UserStaticInfo.Add(userStaticInfo);
                    //_db.UserRoles.Add(new UserRole { RoleId = model.RoleId, UserId = user.Id});
                    //_db.UserRegisters.Add(new UserRegister { RegisterId = model.UserRegisterId, UserId = user.Id});
                    //_db.UserAddresses.AddRange(new UserAddress[] {
                    //    new UserAddress { UserId = user.Id, AddressId = model.AddressLevelOneId},
                    //    new UserAddress { UserId = user.Id, AddressId = model.AddressLevelTwoId},
                    //    new UserAddress { UserId = user.Id, AddressId = model.AddressLevelThreeId}
                    //});

                    //if(model.RoleId != 2 && model.RoleId != 3) {
                    //    List<UserProfession> professions = new List<UserProfession>();
                    //    List<UserEducation> educations = new List<UserEducation>();
                    //    foreach (int profId in model.ProfessionIds) {
                    //        professions.Add(new UserProfession { ProfessionId = profId, UserId = user.Id});
                    //    }
                    //    if (professions.Count > 0) {
                    //        _db.UserProfessions.AddRange(professions);
                    //    }
                    //}

                    //_db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Login", message);
                //model.Roles = (User.IsInRole("3")) ? _db.Roles.ToList() : _db.Roles.Where(r => r.Id != 3).ToList();
                //model.UserStatuses = _db.UserStatuses.ToList();
                //model.UserTypes = _db.UserTypes.ToList();
                //int operatorRegisterId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);
                //int[] registerChildren = registerHelper.GetRegisterChildren(new int[] { }, operatorRegisterId);
                //model.UserRegisters = _db.Registers.Where(r => registerChildren.Contains(r.Id)).ToList();
                //model.Addresses = _db.Addresses.ToList();
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

            int operatorRegisterId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);

            if (!await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User,
                UserPersistenceHelperMode.Read, _db, UserPersistenceState.Update, new UserModelCreate { UserId = id });
                       
            UserModelCreate model = userPersistenceHelper.GetModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UserModelCreate model) {

            Dictionary<string, int> actions = new Dictionary<string, int>();
            if (_db.UserRoles.Any(u => u.UserId == id)) {
                actions.Add("roleId", _db.UserRoles.First(u => u.UserId == id).RoleId);
            }
            actions.Add("childId", model.UserRegisterId);

            if (!await _authorizationService.AuthorizeAsync(User, actions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }

            UserPersistenceHelper userPersistenceHelper = new UserPersistenceHelper(HttpContext.User,
                UserPersistenceHelperMode.Write, _db, UserPersistenceState.Update, model);

            string message;
            if (ModelState.IsValid) {
                if (userPersistenceHelper.Persist(out message)){
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", message);
            }

            return RedirectToAction("Index", "User");
        }
    }
}