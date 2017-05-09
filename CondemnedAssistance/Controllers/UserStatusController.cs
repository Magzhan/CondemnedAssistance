using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class UserStatusController : Controller {
        private UserContext _db;

        public UserStatusController(UserContext context) {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<UserStatus> statuses = _db.UserStatuses.ToList();
            return View(statuses);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserStatus model) {
            if (ModelState.IsValid) {
                UserStatus status = _db.UserStatuses.FirstOrDefault(s => s.NormalizedName == model.Name.ToUpper());
                if (status == null) {
                    status = new UserStatus {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };

                    _db.UserStatuses.Add(status);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "UserStatus");
                }
                else {
                    ModelState.AddModelError("", "Status already exists");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) {
            UserStatus status = _db.UserStatuses.FirstOrDefault(s => s.Id == id);
            if (status == null) {
                ModelState.AddModelError("", "Status with that id does not exist");
                return RedirectToAction("Index", "UserStatus");
            }
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, UserStatus model) {
            if (ModelState.IsValid) {
                UserStatus status = _db.UserStatuses.FirstOrDefault(s => s.Id == id);
                int count = _db.UserStatuses.Where(s => s.NormalizedName == model.Name.ToUpper()).Count();
                if (count > 1) {
                    ModelState.AddModelError("", "No duplicates");
                    return View(model);
                }
                if (status != null) {
                    status.Name = model.Name;
                    status.NormalizedName = model.Name.ToUpper();
                    status.Description = model.Description;
                    status.RequestDate = DateTime.Now;
                    status.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.UserStatuses.Attach(status);
                    _db.Entry(status).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index", "UserStatus");
                } else {
                    ModelState.AddModelError("", "No such status exists");
                    return RedirectToAction("Index", "UserStatus");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id) {
            UserStatus status = _db.UserStatuses.FirstOrDefault(s => s.Id == id);
            if (status == null) {
                ModelState.AddModelError("", "No such status exists");
                return RedirectToAction("Index", "UserStatus");
            }
            _db.UserStatuses.Remove(status);
            return RedirectToAction("Index", "UserStatus");
        }
    }
}
