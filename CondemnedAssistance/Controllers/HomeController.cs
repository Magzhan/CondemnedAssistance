using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondemnedAssistance.Controllers {
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller {
        [AllowAnonymous]
        public IActionResult Index() {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error() {
            return View();
        }
    }
}
