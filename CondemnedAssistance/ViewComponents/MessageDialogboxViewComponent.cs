using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class MessageDialogboxViewComponent : ViewComponent {

        public IViewComponentResult Invoke() {
            return View();
        }

    }
}
