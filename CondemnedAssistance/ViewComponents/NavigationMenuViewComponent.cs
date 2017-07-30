using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class NavigationMenuViewComponent : ViewComponent {

        public IViewComponentResult Invoke() {
            if (User.Identity.IsAuthenticated) {
                string role = HttpContext.User.FindFirst(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                string view = "UserView";
                switch (role) {
                    case "3":
                        view = "FullView";
                        break;
                    case "1":
                        view = "UserView";
                        break;
                    case "2":
                        view = "AdminView";
                        break;
                    case "4":
                        view = "CounselorView";
                        break;
                    default:
                        view = "UserView";
                        break;
                }
                return View(view);
            }
            else {
                return View();
            }
        }
    }
}
