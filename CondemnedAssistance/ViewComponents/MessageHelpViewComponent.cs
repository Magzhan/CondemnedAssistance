using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class MessageHelpViewComponent : ViewComponent{

        private readonly UserContext _db;

        public MessageHelpViewComponent(UserContext context) {
            _db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            if (User.IsInRole("2"))
                return View("Empty");
            return View(await _db.Helps.ToListAsync());
        }

    }
}
