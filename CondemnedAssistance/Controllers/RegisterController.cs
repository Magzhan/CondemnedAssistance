using CondemnedAssistance.Models;
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
    }
}
