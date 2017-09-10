using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class MessageController : Controller {

        public IActionResult Index(){

            return View("Messages");
        }
    }
}
