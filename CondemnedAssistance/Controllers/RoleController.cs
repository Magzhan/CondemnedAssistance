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
    [Authorize(Roles = "3")]
    public class RoleController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;

        public RoleController(UserContext context) {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<RoleModel> roles = new List<RoleModel>();
            
            foreach(var role in _db.Roles) {
                roles.Add(new RoleModel {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                });
            }            
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleModel model) {
            if (ModelState.IsValid) {
                Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == model.Name);
                if (role == null) {
                    _db.Roles.Add(new Role {
                        Name = model.Name,
                        Description = model.Description,
                        NormalizedName = model.Name.ToUpper(),
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    });
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "Role");
                } else {
                    ModelState.AddModelError("", "Following Role already exists");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) {
            Role role = _db.Roles.FirstOrDefault(r => r.Id == id);
            
            if (role == null) {
                ModelState.AddModelError("", "Role with following id does not exist");
                return RedirectToAction("Index", "Role");
            }
            RoleModel model = new RoleModel() {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RoleModel model) {
            if (ModelState.IsValid) {
                Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (role != null) {
                    role.Name = model.Name;
                    role.NormalizedName = model.Name.ToUpper();
                    role.Description = model.Description;
                    role.RequestDate = DateTime.Now;
                    role.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Roles.Attach(role);
                    _db.Entry(role).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "Role");
                } else {
                    ModelState.AddModelError("", "User with current id does not exist");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Role");
        }
    }
}
