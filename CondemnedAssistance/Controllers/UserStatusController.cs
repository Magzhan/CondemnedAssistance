using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserStatus model) {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) {
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, UserStatus model) {
            return View();
        }

        public IActionResult Delete(int id) {
            return RedirectToAction("Index", "UserStatus");
        }
    }
}
