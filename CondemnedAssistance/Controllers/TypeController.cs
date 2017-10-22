using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "3")]
    public class TypeController : Microsoft.AspNetCore.Mvc.Controller {
        private UserContext _db;

        public TypeController(UserContext context) {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<Models.Type> types = _db.Types.ToList();
            return View(types);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.Type model) {
            if (ModelState.IsValid) {
                Models.Type type = _db.Types.FirstOrDefault(t => t.NormalizedName == model.Name.ToUpper());
                if (type == null) {
                    type = new Models.Type {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };

                    _db.Types.Add(type);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "UserType");
                }
                else {
                    ModelState.AddModelError("", "Status already exists");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) {
            Models.Type type = _db.Types.FirstOrDefault(s => s.Id == id);
            if (type == null) {
                ModelState.AddModelError("", "Status with that id does not exist");
                return RedirectToAction("Index", "UserType");
            }
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Models.Type model) {
            if (ModelState.IsValid) {
                Models.Type type = _db.Types.FirstOrDefault(s => s.Id == id);
                int count = _db.Types.Where(s => s.NormalizedName == model.Name.ToUpper()).Count();
                if (count > 1) {
                    ModelState.AddModelError("", "No duplicates");
                    return View(model);
                }
                if (type != null) {
                    type.Name = model.Name;
                    type.NormalizedName = model.Name.ToUpper();
                    type.Description = model.Description;
                    type.RequestDate = DateTime.Now;
                    type.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Types.Attach(type);
                    _db.Entry(type).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index", "UserType");
                } else {
                    ModelState.AddModelError("", "No such status exists");
                    return RedirectToAction("Index", "UserType");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            Models.Type type = _db.Types.FirstOrDefault(s => s.Id == id);
            if (type == null) {
                ModelState.AddModelError("", "No such status exists");
                return RedirectToAction("Index", "UserType");
            }
            _db.Types.Remove(type);
            return RedirectToAction("Index", "UserType");
        }
    }
}
