using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class UserController : Controller {

        private UserContext _db;

        public UserController(UserContext context) {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            
            return View();
        }
    }
}
