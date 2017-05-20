using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class AddressController : Controller {
        private UserContext _db;

        public AddressController(UserContext context) {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Register model) {
            if (ModelState.IsValid) {
                Register address = _db.Registers.FirstOrDefault(a => a.NormalizedName == model.Name.ToUpper());
                if (address == null) {
                    address = new Register();
                    address.Name = model.Name;
                    address.Description = model.Description;
                    address.NormalizedName = model.Name.ToUpper();
                    address.RequestDate = DateTime.Now;
                    address.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _db.Registers.Add(address);
                    _db.SaveChanges();
                } else {
                    ModelState.AddModelError("", "Such address already exists");
                }
            }
            return RedirectToAction("Index", "Address");
        }

        [HttpGet]
        public IActionResult RegisterLevels() {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRegisterLevel() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRegisterLevel(RegisterLevelModel model) {
            if (ModelState.IsValid) {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.NormalizedName.Equals(model.Name.ToUpper()));
                if (registerLevel == null) {
                    registerLevel = new RegisterLevel {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };
                    _db.RegisterLevels.Add(registerLevel);
                    _db.SaveChanges();
                    return RedirectToAction("RegisterLevels", "Register");
                }
                else {
                    ModelState.AddModelError("", "Already exists");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateRegisterLevel(int id) {
            RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
            RegisterLevelModel model = null;
            if (registerLevel != null){
                model = new RegisterLevelModel {
                    Id = registerLevel.Id,
                    Name = registerLevel.Name,
                    Description = registerLevel.Description
                };
            }
            else {
                return RedirectToAction("RegisterLevels", "Register");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateRegisterLevel(int id, RegisterLevelModel model) {
            if (ModelState.IsValid) {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
                if (registerLevel != null) {
                    registerLevel.Name = model.Name;
                    registerLevel.NormalizedName = model.Name.ToUpper();
                    registerLevel.Description = model.Description;
                    registerLevel.RequestDate = DateTime.Now;
                    registerLevel.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.RegisterLevels.Attach(registerLevel);
                    _db.Entry(registerLevel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteRegisterLevel(int id) {
            return RedirectToAction("", "");
        }
    }
}
