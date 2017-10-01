using CondemnedAssistance.Models;
using CondemnedAssistance.Services.IService;
using CondemnedAssistance.Services.Security._Constants;
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
    public class SmsController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public SmsController(UserContext context, IAuthorizationService authorizationService) {
            _db = context;
            _authorizationService = authorizationService;
            _controllerId = _db.Controllers.Single(c => c.NormalizedName == Constants.Sms.ToUpper()).Id;
        }

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
