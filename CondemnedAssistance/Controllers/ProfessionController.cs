using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2,3")]
    public class ProfessionController : Controller{

        private UserContext _db;

        public ProfessionController(UserContext context) {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            return View(_db.Professions.ToList());
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Profession model) {
            if (ModelState.IsValid) {
                if (!_db.Professions.Any(p => p.NormalizedName == model.Name.ToUpper())){
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _db.Professions.Add(model);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id) {
            Profession model = _db.Professions.First(p => p.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int id, Profession model) {
            if (ModelState.IsValid) {
                if(!_db.Professions.Any(p => p.Id != id && p.NormalizedName == model.Name.ToUpper())) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Professions.Attach(model);
                    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            return View();
        }
    }
}
