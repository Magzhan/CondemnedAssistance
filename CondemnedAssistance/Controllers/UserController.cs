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
            ICollection<UserStatus> userStatuses = _db.UserStatuses.ToList();
            ICollection<UserType> userTypes = _db.UserTypes.ToList();
            ICollection<Role> roles = (User.IsInRole("3")) ? _db.Roles.ToList() : _db.Roles.Where(r => r.Id != 3).ToList();
            int operatorRegisterId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);
            int[] registerChildren = registerHelper.GetRegisterChildren(new int[] { }, operatorRegisterId);
            ICollection<Register> registers = _db.Registers.Where(r => registerChildren.Contains(r.Id)).ToList();
            ICollection<Address> addresses = _db.Addresses.ToList();

            if (User.IsInRole("3")) {
                _db.Registers.ForEachAsync(r => registers.Add(r));
            }

            model.Roles = roles;
            model.UserStatuses = userStatuses;
            model.UserTypes = userTypes;
            model.UserRegisters = registers;
            model.Addresses = addresses;
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

            if (ModelState.IsValid) {
                if (!_db.Users.Any(u => u.Login == model.Login)) {
                    User user = new User {
                        Login = model.Login,
                        Email = model.Email,
                        NormalizedEmail = model.Email.ToUpper(),
                        EmailConfirmed = false,
                        PhoneNumber = model.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        AccessFailedCount = 0,
                        LockoutEnabled = false,
                        PasswordHash = "123456",
                        RegistratedUserId = Convert.ToInt32(HttpContext.User.Identity.Name),
                        RegistrationDate = DateTime.Now,
                        ModifiedUserDate = DateTime.Now,
                        ModifiedUserId = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();

                    UserStaticInfo userStaticInfo = new UserStaticInfo {
                        UserId = user.Id,
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        Birthdate = model.Birthdate,
                        Gender = model.Gender,
                        Xin = model.Login,
                        MainAddress = model.MainAddress,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                        UserTypeId = model.UserTypeId,
                        UserStatusId = model.UserStatusId
                    };

                    _db.UserStaticInfo.Add(userStaticInfo);
                    _db.UserRoles.Add(new UserRole { RoleId = model.RoleId, UserId = user.Id});
                    _db.UserRegisters.Add(new UserRegister { RegisterId = model.UserRegisterId, UserId = user.Id});
                    _db.UserAddresses.AddRange(new UserAddress[] {
                        new UserAddress { UserId = user.Id, AddressId = model.AddressLevelOneId},
                        new UserAddress { UserId = user.Id, AddressId = model.AddressLevelTwoId},
                        new UserAddress { UserId = user.Id, AddressId = model.AddressLevelThreeId}
                    });
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Login", "Already exists");
                model.Roles = (User.IsInRole("3")) ? _db.Roles.ToList() : _db.Roles.Where(r => r.Id != 3).ToList();
                model.UserStatuses = _db.UserStatuses.ToList();
                model.UserTypes = _db.UserTypes.ToList();
                int operatorRegisterId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);
                int[] registerChildren = registerHelper.GetRegisterChildren(new int[] { }, operatorRegisterId);
                model.UserRegisters = _db.Registers.Where(r => registerChildren.Contains(r.Id)).ToList();
                model.Addresses = _db.Addresses.ToList();
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

            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            UserStaticInfo userStaticInfo = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == id);
            UserRole myRole = _db.UserRoles.FirstOrDefault(u => u.UserId == id);
            ICollection<UserStatus> userStatuses = _db.UserStatuses.ToList();
            ICollection<UserType> userTypes = _db.UserTypes.ToList();
            ICollection<Role> roles = (User.IsInRole("3")) ? _db.Roles.ToList() : _db.Roles.Where(r => r.Id != 3).ToList();

            if (user == null) {
                return RedirectToAction("Index", "User");
            }

            int[] registerChildren = registerHelper.GetRegisterChildren(new int[] { }, operatorRegisterId);
            ICollection<Register> registers = _db.Registers.Where(r => registerChildren.Contains(r.Id)).ToList();
            UserStatus userStatus = null;
            UserType userType = null;
            UserRegister userRegister = _db.UserRegisters.FirstOrDefault(u => u.UserId == id);
            int[] userAddresses = _db.UserAddresses.Where(a => a.UserId == id).Select(a => a.AddressId).ToArray();

            UserModelCreate model = new UserModelCreate();

            foreach(int address in userAddresses){
                switch(_db.Addresses.First(a => a.Id == address).AddressLevelId) {
                    case 1:
                        model.AddressLevelOneId = address;
                        break;
                    case 2:
                        model.AddressLevelTwoId = address;
                        break;
                    case 3:
                        model.AddressLevelThreeId = address;
                        break;
                }
            }

            if (userRegister != null) {
                model.UserRegisterId = userRegister.RegisterId;
                if (User.IsInRole("3")) {
                    registers.Add(_db.Registers.First(r => r.Id == userRegister.RegisterId));
                }
            } else {
                if (User.IsInRole("3")) {
                    await _db.Registers.ForEachAsync(r => registers.Add(r));
                }
            }
            if (userStaticInfo != null) {
                userStatus = _db.UserStatuses.FirstOrDefault(u => u.Id == userStaticInfo.UserStatusId);
                userType = _db.UserTypes.FirstOrDefault(u => u.Id == userStaticInfo.UserTypeId);
            }
            
            Role role = null;
            if (myRole != null) {
                role = _db.Roles.FirstOrDefault(r => r.Id == myRole.RoleId);
            }

            model.UserId = id;
            model.Login = user.Login;
            model.PhoneNumber = user.PhoneNumber;
            model.Email = user.Email;
            if(userStaticInfo != null) {
                model.LastName = userStaticInfo.LastName;
                model.FirstName = userStaticInfo.FirstName;
                model.MiddleName = userStaticInfo.MiddleName;
                model.Xin = userStaticInfo.Xin;
                model.Birthdate = userStaticInfo.Birthdate;
                model.Gender = userStaticInfo.Gender;
                model.MainAddress = userStaticInfo.MainAddress;
            }
            else {
                model.Gender = true;
            }
            
            model.Roles = roles;
            model.UserStatuses = userStatuses;
            model.UserTypes = userTypes;
            model.UserRegisters = registers;
            model.Addresses = _db.Addresses.ToList();

            if (role != null) {
                model.RoleId = role.Id;
            }
            if(userStatus != null) {
                model.UserStatusId = userStatus.Id;
            }
            if(userType != null) {
                model.UserTypeId = userType.Id;
            }           

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

            if (ModelState.IsValid) {
                using(var t = _db.Database.BeginTransaction()){
                    try {
                        UserStaticInfo userStaticInfo = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == id);
                        UserStatus userStatus = _db.UserStatuses.FirstOrDefault(u => u.Id == model.UserStatusId);
                        UserType userType = _db.UserTypes.FirstOrDefault(u => u.Id == model.UserTypeId);
                        UserRegister userRegister = _db.UserRegisters.FirstOrDefault(u => u.UserId == id);
                        Register register = _db.Registers.FirstOrDefault(r => r.Id == model.UserRegisterId);
                        Role role = _db.Roles.FirstOrDefault(r => r.Id == model.RoleId);
                        User user = _db.Users.FirstOrDefault(u => u.Id == id);
                        UserRole myRole = _db.UserRoles.FirstOrDefault(u => u.UserId == id);
                        UserAddress[] userAddress = _db.UserAddresses.Where(a => a.UserId == id).ToArray();
                        
                        if(userStaticInfo == null) {
                            userStaticInfo = new UserStaticInfo {
                                UserId = id,
                                LastName = model.LastName,
                                FirstName = model.FirstName,
                                MiddleName = model.MiddleName,
                                Xin = model.Xin,
                                Birthdate = model.Birthdate,
                                Gender = model.Gender,
                                UserStatus = userStatus,
                                UserType = userType,
                                User = user,
                                MainAddress = model.MainAddress,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                                RequestDate = DateTime.Now
                            };
                            _db.UserStaticInfo.Add(userStaticInfo);
                        }
                        else {
                            userStaticInfo.LastName = model.LastName;
                            userStaticInfo.FirstName = model.FirstName;
                            userStaticInfo.MiddleName = model.MiddleName;
                            userStaticInfo.Xin = model.Xin;
                            userStaticInfo.Birthdate = model.Birthdate;
                            userStaticInfo.Gender = model.Gender;
                            userStaticInfo.UserStatus = userStatus;
                            userStaticInfo.UserType = userType;
                            userStaticInfo.MainAddress = model.MainAddress;
                            userStaticInfo.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                            userStaticInfo.RequestDate = DateTime.Now;

                            _db.UserStaticInfo.Attach(userStaticInfo);
                            _db.Entry(userStaticInfo).State = EntityState.Modified;
                        }
                        if (myRole == null) {
                            if (role != null) {
                                myRole = new UserRole {
                                    UserId = id,
                                    RoleId = role.Id
                                };
                                
                                _db.UserRoles.Add(myRole);
                            }
                        }
                        else {
                            myRole.RoleId = role.Id;
                            myRole.Role = role;

                            _db.UserRoles.Attach(myRole);
                            _db.Entry(myRole).State = EntityState.Modified;
                        }
                        if (userRegister == null) {
                            if (register != null) {
                                userRegister = new UserRegister {
                                    UserId = id,
                                    RegisterId = register.Id,
                                };
                            }
                            _db.UserRegisters.Add(userRegister);
                        } else {
                            userRegister.RegisterId = register.Id;

                            _db.UserRegisters.Attach(userRegister);
                            _db.Entry(userRegister).State = EntityState.Modified;
                        }

                        if (userAddress.Length > 0) {
                            foreach(UserAddress address in userAddress) {
                                switch(_db.Addresses.First(a => a.Id == address.AddressId).AddressLevelId) {
                                    case 1:
                                        address.AddressId = model.AddressLevelOneId;
                                        break;
                                    case 2:
                                        address.AddressId = model.AddressLevelTwoId;
                                        break;
                                    case 3:
                                        address.AddressId = model.AddressLevelThreeId;
                                        break;
                                }
                                _db.UserAddresses.Attach(address);
                                _db.Entry(address).State = EntityState.Modified;
                            }
                        }
                        _db.SaveChanges();
                        t.Commit();
                    } catch (Exception e) {
                        t.Rollback();
                        ModelState.AddModelError("", e.Message.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "User");
        }
    }
}
