using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class SideBarViewComponent : ViewComponent{

        private readonly UserContext _db;

        public SideBarViewComponent(UserContext context) {
            _db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string [] elements) {
            var thisUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(HttpContext.User.Identity.Name));
            return View();
        }

    }
}
