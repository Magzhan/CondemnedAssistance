using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Event;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class EventController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private ApplicationContext _app;
        private IAuthorizationService _authService;
        private LinkHelper linkHelper;
        private int _controllerId;

        public EventController(UserContext context, ApplicationContext app, IAuthorizationService authService) {
            _db = context;
            _app = app;
            _authService = authService;
            linkHelper = new LinkHelper(context, "userEdit");
            _controllerId = _app.Controllers.Single(c => c.NormalizedName == Constants.Event.ToUpper()).Id;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int userId) {
            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            EventModel model = new EventModel {
                UserId = userId
            };

            await _app.Events.Where(e => e.UserId == userId).ForEachAsync(row => {
                model.Events.Add(_app.Events.Single(r => r.Id == row.Id));
            });

            model.Events = model.Events.OrderByDescending(e => e.Date).ToList();

            model.Events.ForEach(row => {
                row.EventStatus = _app.EventStatuses.Single(e => e.Id == row.EventStatusId);
            });

            DateTime today = DateTime.Now;
            model.CurrentUserStatus = _app.EventStatuses.Single(e => e.Id == 1).Name;
            model.Events.OrderBy(e => e.Date).ToList().ForEach(row => {
                if (today > row.Date) {
                    model.CurrentUserStatus = _app.EventStatuses.Single(e => e.Id == row.EventStatusId).Name;
                }
            });

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());            

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = false,
                Text = "Персональные данные",
                RouteValues = routeVals
            });
            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = true,
                Text = "История",
                RouteValues = routeVals1
            });
            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = true,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int userId) {
            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            EventCreateModel model = new EventCreateModel {
                EventStatuses = _app.EventStatuses.ToList()
            };

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());            

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = false,
                Text = "Персональные данные",
                RouteValues = routeVals
            });
            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = true,
                Text = "История",
                RouteValues = routeVals1
            });
            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = true,
                Text = "Пробация",
                RouteValues = routeVals2
            });
            
            ViewData["sidebar"] = links.ToArray();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, EventCreateModel model) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                if (!_app.Events.Where(e => e.UserId == userId).Any(row => _app.Events.Single(r => r.Id == row.Id).Date == model.Date)) {
                   Event myEvent = new Event {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        Date = model.Date,
                        EventStatusId = model.EventStatusId,
                        UserId = userId,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };

                    await _app.Events.AddAsync(myEvent);
                    await _app.SaveChangesAsync();

                    return RedirectToAction("Index", new { userId = userId });
                }
                ModelState.AddModelError("", "Events with same time for the same user cannot be added");
            }

            model.EventStatuses = _app.EventStatuses.ToList();

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = false,
                Text = "Персональные данные",
                RouteValues = routeVals
            });
            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = true,
                Text = "История",
                RouteValues = routeVals1
            });
            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = true,
                Text = "Пробация",
                RouteValues = routeVals2
            });
            ViewData["sidebar"] = links.ToArray();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id){

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            int userId = _app.Events.Single(e => e.Id == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            Event thisEvent = await _app.Events.SingleAsync(e => e.Id == id);

            EventCreateModel model = new EventCreateModel {
                UserId = userId,
                Id = id,
                Name = thisEvent.Name,
                Description = thisEvent.Description,
                EventStatusId = thisEvent.EventStatusId,
                Date = thisEvent.Date,
                EventStatuses = _app.EventStatuses.ToList()
            };

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());            

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = false,
                Text = "Персональные данные",
                RouteValues = routeVals
            });
            Dictionary<string, string> routeVals1 = new Dictionary<string, string> { };
            routeVals1.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "User",
                Action = "History",
                IsSelected = true,
                Text = "История",
                RouteValues = routeVals1
            });
            Dictionary<string, string> routeVals2 = new Dictionary<string, string> { };
            routeVals2.Add("userId", userId.ToString());
            links.Add(new LinkClass {
                Controller = "Event",
                Action = "Index",
                IsSelected = true,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, EventCreateModel model) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            int userId = _app.Events.Single(e => e.Id == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                if (!_app.Events.Where(e => e.UserId == userId && e.Id != id).Any(row => _app.Events.Single(r => r.Id == row.Id).Date == model.Date)) {
                    
                    Event thisEvent = await _app.Events.SingleAsync(e => e.Id == id);
                    thisEvent.Name = model.Name;
                    thisEvent.NormalizedName = model.Name.ToUpper();
                    thisEvent.Description = model.Description;
                    thisEvent.Date = model.Date;
                    thisEvent.EventStatusId = model.EventStatusId;
                    thisEvent.RequestDate = DateTime.Now;
                    thisEvent.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _app.Events.Attach(thisEvent);
                    _app.Entry(thisEvent).State = EntityState.Modified;

                    await _app.SaveChangesAsync();

                    return RedirectToAction("Index", new { userId = userId });
                }
                ModelState.AddModelError("", "Already has event with same date"); 
            }

            model.EventStatuses = _app.EventStatuses.ToList();

            Dictionary<string, string> routeVals = new Dictionary<string, string> { };
            routeVals.Add("id", userId.ToString());

            List<LinkClass> links = linkHelper.GetLinks("User", "Update").ToList();
            links.Add(new LinkClass {
                Action = "Update",
                Controller = "User",
                IsSelected = false,
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
                IsSelected = true,
                Text = "Пробация",
                RouteValues = routeVals2
            });

            ViewData["sidebar"] = links.ToArray();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.Delete);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            int userId = _app.Events.Single(e => e.Id == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            Event thisEvent = await _app.Events.SingleAsync(e => e.Id == id);
            _app.Events.Remove(thisEvent);

            return RedirectToAction("User", "Index");
        }

        [HttpGet]
        public async Task<IActionResult> EventStatuses() {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.EventStatuses);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            List<EventStatus> model = _app.EventStatuses.ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateStatus() {
            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.CreateStatus);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatus(EventStatus model) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.CreateStatus);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                if (!_app.EventStatuses.Any(s => s.NormalizedName == model.Name.ToUpper())) {
                    _app.EventStatuses.Add(new EventStatus {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                        RequestDate = DateTime.Now
                    });

                    _app.SaveChanges();
                    return RedirectToAction("EventStatuses");
                }
                ModelState.AddModelError("", "Already has such value");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id){

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.UpdateStatus);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            EventStatus model = _app.EventStatuses.Single(s => s.Id == id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, EventStatus model) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.UpdateStatus);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                if (!_app.EventStatuses.Any(s => s.Id != id && s.NormalizedName == model.Name.ToUpper())) {
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _app.EventStatuses.Attach(model);
                    _app.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    _app.SaveChanges();
                    return RedirectToAction("EventStatuses");
                }
                ModelState.AddModelError("", "Already has such item");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStatus(int id) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.DeleteStatus);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            if(!_app.Events.Any(e => e.EventStatusId == id)) {
                EventStatus model = _app.EventStatuses.Single(e => e.Id == id);
                _app.EventStatuses.Remove(model);
                _app.SaveChanges();
            }
            return RedirectToAction("EventStatuses");
        }
    }
}
 