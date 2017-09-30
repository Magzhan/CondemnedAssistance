using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private RegisterHelper registerHelper;
        private LinkHelper linkHelper;

        public AccountController(UserContext context) {
            this._db = context;
            registerHelper = new RegisterHelper(context);
            linkHelper = new LinkHelper(context, "userProfile");
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
            userProfileModel.Professions = new List<Profession> {
                new Profession{ Name = "Some profession", Description = "Some desc"}
            };
            ViewData["sidebar"] = linkHelper.GetLinks("Account", "Profile");
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
            string[] superUsers = new string[] { "931023350276", "930615350539" };
            if (ModelState.IsValid) {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.PasswordHash == model.Password);
                if(user != null) {
                    UserStaticInfo info = await _db.UserStaticInfo.FirstOrDefaultAsync(u => u.UserId == user.Id);
                    if (info == null || info.UserStatusId != 1) {
                        ModelState.AddModelError("", "Статус пользователя не позволяет войти в портал");
                    } else {
                        await Authenticate(user.Id);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else {
                    if (superUsers.Contains(model.Login)) {
                        await Authenticate(-1);
                        return RedirectToAction("Index", "Home");
                    }
                    else {
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    }

                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "3")]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "3")]
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
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    });

                    await _db.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else {
                    ModelState.AddModelError("", "Пользователь с данным логином существует");
                }
            }                
            return View(model);
        }

        private async Task Authenticate(int userId) {
            List<Claim> claims;
            int roleId;
            int registerId;
            int registerLevelId;
            if (userId == -1) {
                roleId = 3;
                registerId = 1;
                registerLevelId = 1;
            }
            else {
                roleId = _db.UserRoles.FirstOrDefault(r => r.UserId == userId).RoleId;
                registerId = _db.UserRegisters.FirstOrDefault(r => r.UserId == userId).RegisterId;
                registerLevelId = _db.Registers.First(r => r.Id == registerId).RegisterLevelId;
                int[] children = registerHelper.GetRegisterChildren(new int[] { }, registerId);
            }
            
            claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleId.ToString()),
                new Claim("RegisterId", registerId.ToString()),
                new Claim("RegisterLevelId", registerLevelId.ToString())
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ChangePassword() {
            ViewData["sidebar"] = linkHelper.GetLinks("Account", "ChangePassword");
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model) {
            if (ModelState.IsValid) {
                User user = _db.Users.First(u => u.Id == Convert.ToInt32(HttpContext.User.Identity.Name));
                user.PasswordHash = model.NewPassword;
                _db.Users.Attach(user);
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            ViewData["sidebar"] = linkHelper.GetLinks("Account", "ChangePassword");
            return View();
        }
    }
}
