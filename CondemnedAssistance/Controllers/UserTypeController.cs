using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class UserTypeController : Controller {
        private UserContext _db;

        public UserTypeController(UserContext context) {
            this._db = context;
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
        public IActionResult Create(UserType model) {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) {
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, UserType model) {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id) {
            return RedirectToAction("Index", "UserType");
        }
    }
}
