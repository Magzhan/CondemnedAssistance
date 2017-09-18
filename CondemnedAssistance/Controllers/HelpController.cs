using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "3")]
    public class HelpController : Controller {

        private UserContext _db;
        private LinkHelper linkHelper;

        public HelpController(UserContext context) {
            _db = context;
            linkHelper = new LinkHelper(context, "userEdit");
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
        public IActionResult Index() {
            List<Help> model = _db.Helps.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Help model) {
            if (ModelState.IsValid) {
                if(!_db.Helps.Any(h => h.NormalizedName == model.Name.ToUpper())) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Helps.Add(model);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already has such topic");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id) {
            Help model = _db.Helps.Single(h => h.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int id, Help model) {
            if (ModelState.IsValid) {
                if(!_db.Helps.Any(h => h.NormalizedName == model.Name.ToUpper() && h.Id != id)) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Helps.Attach(model);
                    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already has such topic");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            if (_db.Helps.Any(h => h.Id == id)) {
                Help help = _db.Helps.Single(h => h.Id == id);
                _db.Helps.Remove(help);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UserHelpList(int userId) {
            int[] userHelpList = _db.UserHelps.Where(h => h.UserId == userId).Select(h => h.HelpId).ToArray();
            return View(_db.Helps.Where(h => userHelpList.Contains(h.Id)).ToList());
        }

        [HttpGet]
        public IActionResult AddUserHelp(int userId) {

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

            return View(new HelpModel { UserId = userId, Helps = _db.Helps.ToList(), HelpIds = _db.UserHelps.Where(h => h.UserId == userId).Select(h => h.HelpId).ToArray() });
        }

        [HttpPost]
        public IActionResult AddUserHelp(int userId, HelpModel model) {
            if (ModelState.IsValid) {
                if (_db.UserHelps.Any(h => h.UserId == userId)) {
                    foreach (UserHelp help in _db.UserHelps.Where(h => h.UserId == userId)) {
                        _db.UserHelps.Remove(help);
                    }
                    _db.SaveChanges();
                }
                List<UserHelp> userHelps = new List<UserHelp>();
                foreach(int id in model.HelpIds) {
                    userHelps.Add(new UserHelp { UserId = userId, HelpId = id });
                }
                _db.UserHelps.AddRange(userHelps);
                _db.SaveChanges();
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
