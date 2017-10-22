using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "3")]
    public class StatusController : Microsoft.AspNetCore.Mvc.Controller {
        private UserContext _db;

        public StatusController(UserContext context) {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            ICollection<Status> statuses = _db.Statuses.ToList();
            return View(statuses);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Status model) {
            if (ModelState.IsValid) {
                Status status = _db.Statuses.FirstOrDefault(s => s.NormalizedName == model.Name.ToUpper());
                if (status == null) {
                    status = new Status {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };

                    _db.Statuses.Add(status);
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
            Status status = _db.Statuses.FirstOrDefault(s => s.Id == id);
            if (status == null) {
                ModelState.AddModelError("", "Status with that id does not exist");
                return RedirectToAction("Index", "UserStatus");
            }
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Status model) {
            if (ModelState.IsValid) {
                Status status = _db.Statuses.FirstOrDefault(s => s.Id == id);
                int count = _db.Statuses.Where(s => s.NormalizedName == model.Name.ToUpper()).Count();
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

                    _db.Statuses.Attach(status);
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

        [HttpDelete]
        public IActionResult Delete(int id) {
            Status status = _db.Statuses.FirstOrDefault(s => s.Id == id);
            if (status == null) {
                ModelState.AddModelError("", "No such status exists");
                return RedirectToAction("Index", "UserStatus");
            }
            _db.Statuses.Remove(status);
            return RedirectToAction("Index", "UserStatus");
        }
    }
}
