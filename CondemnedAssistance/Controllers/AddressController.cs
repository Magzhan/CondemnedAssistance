using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2, 3")]
    public class AddressController : Controller{

        private UserContext _db;

        public AddressController(UserContext context) {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Create(int levelId, int parentId, int childId) {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddressModel model) {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) {
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, AddressModel model) {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            return View();
        }

        [HttpGet]
        public IActionResult AddressLevels() {
            List<AddressLevelModel> models = new List<AddressLevelModel>();
            foreach(AddressLevel addressLevel in _db.AddressLevels.ToList()) {
                models.Add(new AddressLevelModel {
                    Id = addressLevel.Id,
                    Name = addressLevel.Name,
                    Description = addressLevel.Description
                });
            }
            return View(models);
        }

        [HttpGet]
        public IActionResult CreateLevel() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLevel(AddressLevelModel model) {
            if (ModelState.IsValid) {
                if(!_db.AddressLevels.Any(ad => ad.NormalizedName == model.Name.ToUpper())) {
                    AddressLevel addressLevel = new AddressLevel {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };
                    _db.AddressLevels.Add(addressLevel);
                    _db.SaveChanges();
                    return RedirectToAction("AddressLevels");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View();
        }

        [HttpGet]
        public IActionResult UpdateLevel(int id) {
            if(_db.AddressLevels.Any(ad => ad.Id == id)) {
                AddressLevel addressLevel = _db.AddressLevels.First(a => a.Id == id);
                AddressLevelModel model = new AddressLevelModel {
                    Id = addressLevel.Id,
                    Name = addressLevel.Name,
                    Description = addressLevel.Description
                };
                return View(model);
            }
            return RedirectToAction("AddressLevels");
        }

        [HttpPost]
        public IActionResult UpdateLevel(int id, AddressLevelModel model) {
            if (ModelState.IsValid) {
                if (_db.AddressLevels.Any(a => a.Id == id)) {
                    AddressLevel addressLevel = _db.AddressLevels.First(a => a.Id == id);
                    addressLevel.Name = model.Name;
                    addressLevel.NormalizedName = model.Name.ToUpper();
                    addressLevel.Description = model.Description;
                    addressLevel.RequestDate = DateTime.Now;
                    addressLevel.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.AddressLevels.Attach(addressLevel);
                    _db.Entry(addressLevel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                    return View(model);
                }                
            }
            return RedirectToAction("AddressLevels");
        }

        [HttpGet]
        public IActionResult DeleteLevel(int id) {
            if (_db.Addresses.Any(a => a.AddressLevelId == id)) {
                ModelState.AddModelError("", "It still has binded elements");
                return RedirectToAction("AddressLevels");
            }
            AddressLevel addressLevel = _db.AddressLevels.First(a => a.Id == id);
            _db.AddressLevels.Remove(addressLevel);
            return RedirectToAction("AddressLevels");
        }
    }
}
