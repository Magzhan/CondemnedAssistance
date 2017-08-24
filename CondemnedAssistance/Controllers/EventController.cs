using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2,3")]
    public class EventController : Controller{

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(int id) {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id){
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, int idd) {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            return RedirectToAction("User", "Index");
        }
    }
}
