﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondemnedAssistance.Controllers {
    public class HomeController : Controller {
        [AllowAnonymous]
        public IActionResult Index() {
            return View();
        }
    }
}
