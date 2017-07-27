using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2, 3")]
    public class EducationController : Controller{

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

        [HttpPost]
        public IActionResult Delete(int id) {
            return View();
        }

        [HttpGet]
        public IActionResult EducationLevels() {
            return View();
        }

        [HttpGet]
        public IActionResult CreateEducationLevel() {
            return View();
        }

        //[HttpPost]
        //public IActionResult CreateEducationLevel() {
        //    return View();
        //}

        [HttpGet]
        public IActionResult UpdateEducationLevel(int id) {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateEducationLevel() {
            return View();
        }

        public IActionResult DeleteEducationLevel(int id) {
            return View();
        }
    }
}
