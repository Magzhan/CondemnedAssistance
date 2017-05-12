using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class UserController : Controller {

        private UserContext _db;

        public UserController(UserContext context) {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<User> users = _db.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult View(int id) {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            UserStaticInfo userStaticInfo = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == id);
            UserRole userRole = _db.UsersRoles.FirstOrDefault(u => u.UserId == id);
            UserStatus userStatus = _db.UserStatuses.FirstOrDefault(u => u.Id == userStaticInfo.UserStatusId);
            UserType userType = _db.UserTypes.FirstOrDefault(u => u.Id == userStaticInfo.UserTypeId);
            Role role = _db.Roles.FirstOrDefault(r => r.Id == userRole.Id);

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
        public IActionResult Update(int id) {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            UserStaticInfo userStaticInfo = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == id);
            UserRole userRole = _db.UsersRoles.FirstOrDefault(u => u.UserId == id);
            ICollection<UserStatus> userStatuses = _db.UserStatuses.ToList();
            ICollection<UserType> userTypes = _db.UserTypes.ToList();
            ICollection<Role> roles = _db.Roles.ToList();
            UserStatus userStatus = _db.UserStatuses.FirstOrDefault(u => u.Id == userStaticInfo.UserStatusId);
            UserType userType = _db.UserTypes.FirstOrDefault(u => u.Id == userStaticInfo.UserTypeId);
            Role role = _db.Roles.FirstOrDefault(r => r.Id == userRole.Id);

            if (user == null) {
                return RedirectToAction("Index", "User");
            }

            UserModelModify model = new UserModelModify();

            model.UserId = id;
            model.Login = user.Login;
            model.LastName = userStaticInfo.LastName;
            model.FirstName = userStaticInfo.FirstName;
            model.MiddleName = userStaticInfo.MiddleName;
            model.Xin = userStaticInfo.Xin;
            model.Birthdate = userStaticInfo.Birthdate;
            model.Gender = userStaticInfo.Gender;            
            model.Roles = roles;
            model.UserStatuses = userStatuses;
            model.UserTypes = userTypes;

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
        public IActionResult Update(int id, UserModelModify model) {

            if (ModelState.IsValid) {
                using(var t = _db.Database.BeginTransaction()){
                    try {
                        UserStaticInfo userStaticInfo = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == id);
                        UserStatus userStatus = _db.UserStatuses.FirstOrDefault(u => u.Id == model.UserStatusId);
                        UserType userType = _db.UserTypes.FirstOrDefault(u => u.Id == model.UserTypeId);
                        Role role = _db.Roles.FirstOrDefault(r => r.Id == model.RoleId);
                        User user = _db.Users.FirstOrDefault(u => u.Id == id);
                        UserRole userRole = _db.UsersRoles.FirstOrDefault(u => u.UserId == id);

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
                            userStaticInfo.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                            userStaticInfo.RequestDate = DateTime.Now;

                            _db.UserStaticInfo.Attach(userStaticInfo);
                            _db.Entry(userStaticInfo).State = EntityState.Modified;
                        }
                        if (userRole == null) {
                            userRole = new UserRole {
                                Role = role,
                                RoleId = role.Id,
                                User = user,
                                UserId = user.Id
                            };
                            _db.UsersRoles.Add(userRole);
                        }
                        else {
                            userRole.RoleId = role.Id;
                            userRole.Role = role;

                            _db.UsersRoles.Attach(userRole);
                            _db.Entry(userRole).State = EntityState.Modified;
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
