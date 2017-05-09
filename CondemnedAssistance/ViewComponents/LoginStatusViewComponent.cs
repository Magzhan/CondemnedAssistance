using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class LoginStatusViewComponent : ViewComponent {

        private readonly UserContext _db;

        public LoginStatusViewComponent(UserContext context) {
            this._db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            if (User.Identity.IsAuthenticated) {
                int currentUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == currentUser);
                return View("LoggedIn", user);
            } else {
                return View();
            }
        }
    }
}
