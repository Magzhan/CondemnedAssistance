using CondemnedAssistance.Services.IService;
using CondemnedAssistance.Services.Sms;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "3")]
    public class SmsController : Controller{

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Send() {
            SmsModel model = new SmsModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Send(SmsModel model) {
            SmsSender sender = (SmsSender)HttpContext.RequestServices.GetRequiredService<IMessageSender>();
            sender.Send(model.UserId, Convert.ToInt32(HttpContext.User.Identity.Name), model.Subject, model.Text);
            return View();
        }

    }
}
