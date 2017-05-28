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

        public AccountController(UserContext context) {
            this._db = context;
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
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model) {
            if (ModelState.IsValid) {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if(user == null) {
                    _db.Users.Add(new User {
                        Login = model.Login,
                        Email = model.Email,
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
            return View(model);
        }

        private async Task Authenticate(int userId) {
            int roleId = _db.UserRoles.FirstOrDefault(r => r.UserId == userId).RoleId;
            int registerId = _db.UserRegisters.FirstOrDefault(r => r.UserId == userId).RegisterId;
            Role role = _db.Roles.FirstOrDefault(r => r.Id == roleId);
            Register register = _db.Registers.FirstOrDefault(r => r.Id == registerId);
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Id.ToString()),
                new Claim("RegisterId", register.Id.ToString()),
                new Claim("RegisterLevelId", register.RegisterLevelId.ToString())
            };

            int[] children = await getRegisterChildren(registerId, new int[] { });

            foreach(int child in children) {
                claims.Add(new Claim("RegisterChildId", child.ToString()));
            }

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }

        private async Task<int[]> getRegisterChildren(int parentId, int[] children) {
            if (!_db.RegisterHierarchies.Any(r => r.ParentRegister == parentId)) {
                return children;
            } else {
                List<int> allChildren = new List<int>();
                int[] tempChildren = _db.RegisterHierarchies.Where(r => r.ParentRegister == parentId).Select(r => r.ChildRegister).ToArray();
                
                foreach (int child in tempChildren) {
                    allChildren.AddRange(await getRegisterChildren(child, tempChildren));
                }
                return allChildren.Distinct().ToArray();
            }
        }
    }
}
