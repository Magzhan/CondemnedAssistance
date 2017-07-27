using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class ProfessionController : Controller{

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create() {
        //    return View();
        //}

        [HttpGet]
        public IActionResult Update(int id) {
            return View();
        }

        [HttpPost]
        public IActionResult Update() {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            return View();
        }
    }
}
