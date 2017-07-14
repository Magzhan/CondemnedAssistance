﻿using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class HelpController : Controller {

        private UserContext _db;

        public HelpController(UserContext context) {
            _db = context;
        }

        public IActionResult Index(int id) {
            return View();
        }
    }
}