using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class AccountController : Controller {

        private UserContext _db;
        private RegisterHelper registerHelper;

        public AccountController(UserContext context) {
            this._db = context;
            registerHelper = new RegisterHelper(context);
        }

        [HttpGet]
        public IActionResult Profile() {
            var userProfileModel = new UserProfileModel();
            var userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            var user = _db.Users.First(u => u.Id == userId);
            var userStaticData = _db.UserStaticInfo.FirstOrDefault(u => u.UserId == userId);
            var userRegister = _db.UserRegisters.FirstOrDefault(r => r.UserId == userId).RegisterId;
            var register = _db.Registers.FirstOrDefault(r => r.Id == userRegister);
            var userRole = _db.UserRoles.FirstOrDefault(r => r.UserId == userId).RoleId;
            var role = _db.Roles.FirstOrDefault(r => r.Id == userRole);
            var status = _db.UserStatuses.FirstOrDefault(s => s.Id == userStaticData.UserStatusId);

            userProfileModel.Login = user.Login;
            userProfileModel.Role = role.Name;
            userProfileModel.Status = status.Name;
            userProfileModel.LastName = userStaticData.LastName;
            userProfileModel.FirstName = userStaticData.FirstName;
            userProfileModel.MiddleName = userStaticData.MiddleName;
            userProfileModel.Birthdate = userStaticData.Birthdate;
            userProfileModel.Gender = userStaticData.Gender;
            userProfileModel.Xin = userStaticData.Xin;
            userProfileModel.Email = user.Email;
            userProfileModel.IsEmailConfirmed = user.EmailConfirmed;
            userProfileModel.MobilePhone = user.PhoneNumber.ToString();
            userProfileModel.IsMobileConfirmed = user.PhoneNumberConfirmed;
            userProfileModel.Registration = register.Name;
            userProfileModel.Educations = new List<Education> {
                new Education{ Name = "Some school", Description = "Some description", EducationLevel = new EducationLevel{ Id = 0, Name = "Middle" } }
            };
            userProfileModel.Professeions = new List<Profession> {
                new Profession{ Name = "Some profession", Description = "Some desc"}
            };
            return View(userProfileModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) {
            if (ModelState.IsValid) {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.PasswordHash == model.Password);
                if(user != null) {
                    await Authenticate(user.Id);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "3, 2")]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "3, 2")]
        public async Task<IActionResult> Register(RegistrationModel model) {
            if (ModelState.IsValid) {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null) {
                    _db.Users.Add(new User {
                        Login = model.Login,
                        Email = model.Email,
                        NormalizedEmail = model.Email.ToUpper(),
                        PasswordHash = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        ModifiedUserDate = DateTime.Now,
                        ModifiedUserId = Convert.ToInt32(HttpContext.User.Identity.Name),
                        RegistratedUserId = Convert.ToInt32(HttpContext.User.Identity.Name),
                        RegistrationDate = DateTime.Now
                    });

                    await _db.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else {
                    ModelState.AddModelError("", "Пользователь с данным логином существует");
                }
            }
            //else {
            //    var message = "" + ModelState.ErrorCount;
            //    foreach(var i in ModelState.Values) {
            //        if(i.Errors.Count > 0) {
            //            message = message + i.Errors[0].ErrorMessage + " " + i.Errors[0].Exception + "\n";
            //        }
            //    }
            //    throw new Exception(message);
            //}
                
            return View(model);
        }

        private async Task Authenticate(int userId) {
            List<Claim> claims;
                        
            int roleId = _db.UserRoles.FirstOrDefault(r => r.UserId == userId).RoleId;
            int registerId = _db.UserRegisters.FirstOrDefault(r => r.UserId == userId).RegisterId;
            Role role = _db.Roles.FirstOrDefault(r => r.Id == roleId);
            Register register = _db.Registers.FirstOrDefault(r => r.Id == registerId);
            claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Id.ToString()),
                new Claim("RegisterId", register.Id.ToString()),
                new Claim("RegisterLevelId", register.RegisterLevelId.ToString())
            };

            int[] children = registerHelper.GetRegisterChildren(new int[] { }, registerId);

            foreach (int child in children) {
                claims.Add(new Claim("RegisterChildId", child.ToString()));
            }            

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }
    }
}
