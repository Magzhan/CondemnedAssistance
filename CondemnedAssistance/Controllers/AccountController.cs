using Microsoft.AspNetCore.Mvc;

namespace CondemnedAssistance.Controllers {
    public class AccountController : Controller {
        public IActionResult Login() {
            return View();
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }
    }
}
