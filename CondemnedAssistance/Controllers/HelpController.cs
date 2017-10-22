using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Help;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class HelpController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private ApplicationContext _app;
        private LinkHelper linkHelper;
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public HelpController(UserContext context, ApplicationContext app, IAuthorizationService authorizationService) {
            _db = context;
            _app = app;
            linkHelper = new LinkHelper(context, "userEdit");
            _authorizationService = authorizationService;
            _controllerId = _app.Controllers.Single(c => c.NormalizedName == Constants.Help).Id;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PsychoHelp() {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PravoHelp()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MedHelp()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ObrHelp()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ZawitaHelp()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TrudHelp()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AktyHelp()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            List<Help> model = _app.Helps.ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Help model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                if(!_app.Helps.Any(h => h.NormalizedName == model.Name.ToUpper())) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _app.Helps.Add(model);
                    _app.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already has such topic");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Help model = _app.Helps.Single(h => h.Id == id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Help model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                if(!_app.Helps.Any(h => h.NormalizedName == model.Name.ToUpper() && h.Id != id)) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _app.Helps.Attach(model);
                    _app.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _app.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already has such topic");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.Delete);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (_app.Helps.Any(h => h.Id == id)) {
                Help help = _app.Helps.Single(h => h.Id == id);
                _app.Helps.Remove(help);
                _app.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserHelpList(int userId) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.UserHelpList);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            int[] userHelpList = _app.UserHelps.Where(h => h.UserId == userId).Select(h => h.HelpId).ToArray();
            return View(_app.Helps.Where(h => userHelpList.Contains(h.Id)).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> AddUserHelp(int userId) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.AddUserHelp);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = true,
                Text = "Персональные данные",
                RouteValues = routeVals
            });

            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());

            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = false,
                Text = "История",
                RouteValues = routeVals1
            });

            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString()); 
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = false,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return View(new HelpModel { UserId = userId, Helps = _app.Helps.ToList(), HelpIds = _app.UserHelps.Where(h => h.UserId == userId).Select(h => h.HelpId).ToArray() });
        }

        [HttpPost]
        public async Task<IActionResult> AddUserHelp(int userId, HelpModel model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, HelpOperations.AddUserHelp);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                if (_app.UserHelps.Any(h => h.UserId == userId)) {
                    foreach (UserHelp help in _app.UserHelps.Where(h => h.UserId == userId)) {
                        _app.UserHelps.Remove(help);
                    }
                    _app.SaveChanges();
                }
                List<UserHelp> userHelps = new List<UserHelp>();
                foreach(int id in model.HelpIds) {
                    userHelps.Add(new UserHelp { UserId = userId, HelpId = id });
                }
                _app.UserHelps.AddRange(userHelps);
                _app.SaveChanges();
            }

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = true,
                Text = "Персональные данные",
                RouteValues = routeVals
            });

            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());

            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = false,
                Text = "История",
                RouteValues = routeVals1
            });

            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString()); 
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = false,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return RedirectToAction("UserHelpList", new { userId = userId});
        }
    }
}
