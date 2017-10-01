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
        private IAuthorizationService _authService;
        private LinkHelper linkHelper;
        private int _controllerId;

        public EventController(UserContext context, IAuthorizationService authService) {
            _db = context;
            _authService = authService;
            linkHelper = new LinkHelper(context, "userEdit");
            _controllerId = _db.Controllers.Single(c => c.NormalizedName == Constants.Event.ToUpper()).Id;
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

            await _db.UserEvents.Where(e => e.UserId == userId).ForEachAsync(row => {
                model.Events.Add(_db.Events.Single(r => r.Id == row.EventId));
            });

            model.Events = model.Events.OrderByDescending(e => e.Date).ToList();

            model.Events.ForEach(row => {
                row.EventStatus = _db.EventStatuses.Single(e => e.Id == row.EventStatusId);
            });

            DateTime today = DateTime.Now;
            model.CurrentUserStatus = _db.EventStatuses.Single(e => e.Id == 1).Name;
            model.Events.OrderBy(e => e.Date).ToList().ForEach(row => {
                if (today > row.Date) {
                    model.CurrentUserStatus = _db.EventStatuses.Single(e => e.Id == row.EventStatusId).Name;
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
                EventStatuses = _db.EventStatuses.ToList()
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
                if (!_db.UserEvents.Where(e => e.UserId == userId).Any(row => _db.Events.Single(r => r.Id == row.EventId).Date == model.Date)) {
                    _db.Database.AutoTransactionsEnabled = false;
                    using (var t = _db.Database.BeginTransaction()) {
                        try {
                            Guid transactionGuid = t.TransactionId;

                            Transaction transaction = new Transaction { TransactionGuid = transactionGuid };

                            await _db.Transactions.AddAsync(transaction);
                            await _db.SaveChangesAsync();

                            Event myEvent = new Event {
                                Name = model.Name,
                                NormalizedName = model.Name.ToUpper(),
                                Description = model.Description,
                                Date = model.Date,
                                EventStatusId = model.EventStatusId,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                            };
                            await _db.Events.AddAsync(myEvent);
                            await _db.SaveChangesAsync();

                            await _db.UserEvents.AddAsync(new UserEvent {
                                EventId = myEvent.Id,
                                UserId = userId,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                            });

                            await _db.UserEventHistory.AddAsync(new UserEventHistory {
                                EventId = myEvent.Id,
                                UserId = userId,
                                TransactionId = transaction.TransactionId,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                                ActionType = DatabaseActionTypes.Insert
                            });
                            await _db.SaveChangesAsync();

                            UserPersistenceHelper persistenceHelper =
                                new UserPersistenceHelper(_db, transaction, DatabaseActionTypes.Insert, userId);

                            persistenceHelper.PersistHistory();

                            await _db.SaveChangesAsync();

                            t.Commit();
                            return RedirectToAction("Index", new { userId = userId });
                        }
                        catch(Exception ex) {
                            ModelState.AddModelError("", ex.ToString());
                            t.Rollback();
                        }
                    }
                }
                ModelState.AddModelError("", "Events with same time for the same user cannot be added");
            }

            model.EventStatuses = _db.EventStatuses.ToList();

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

            int userId = _db.UserEvents.Single(e => e.EventId == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            Event thisEvent = await _db.Events.SingleAsync(e => e.Id == id);

            EventCreateModel model = new EventCreateModel {
                UserId = userId,
                Id = id,
                Name = thisEvent.Name,
                Description = thisEvent.Description,
                EventStatusId = thisEvent.EventStatusId,
                Date = thisEvent.Date,
                EventStatuses = _db.EventStatuses.ToList()
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

            int userId = _db.UserEvents.Single(e => e.EventId == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                if (!_db.UserEvents.Where(e => e.UserId == userId && e.EventId != id).Any(row => _db.Events.Single(r => r.Id == row.EventId).Date == model.Date)) {
                    _db.Database.AutoTransactionsEnabled = false;
                    using(var t = _db.Database.BeginTransaction()) {
                        try {
                            Guid transactionGuid = t.TransactionId;

                            Transaction transaction = new Transaction { TransactionGuid = transactionGuid };

                            await _db.Transactions.AddAsync(transaction);
                            await _db.SaveChangesAsync();

                            Event thisEvent = await _db.Events.SingleAsync(e => e.Id == id);
                            thisEvent.Name = model.Name;
                            thisEvent.NormalizedName = model.Name.ToUpper();
                            thisEvent.Description = model.Description;
                            thisEvent.Date = model.Date;
                            thisEvent.EventStatusId = model.EventStatusId;
                            thisEvent.RequestDate = DateTime.Now;
                            thisEvent.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                            _db.Events.Attach(thisEvent);
                            _db.Entry(thisEvent).State = EntityState.Modified;

                            await _db.UserEventHistory.AddAsync(new UserEventHistory {
                                EventId = thisEvent.Id,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                                ActionType = DatabaseActionTypes.Update,
                                TransactionId = transaction.TransactionId,
                                UserId = userId,
                            });

                            await _db.SaveChangesAsync();

                            UserPersistenceHelper persistenceHelper =
                                new UserPersistenceHelper(_db, transaction, DatabaseActionTypes.Update, userId);

                            t.Commit();
                            return RedirectToAction("Index", new { userId = userId });
                        }
                        catch (Exception ex) {
                            ModelState.AddModelError("", ex.ToString());
                            t.Rollback();
                        }
                    }
                }
                ModelState.AddModelError("", "Already has event with same date"); 
            }

            model.EventStatuses = _db.EventStatuses.ToList();

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

            int userId = _db.UserEvents.Single(e => e.EventId == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int> {
                { "childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId }
            };

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            _db.Database.AutoTransactionsEnabled = false;

            using(var t = _db.Database.BeginTransaction()) {
                try {
                    Guid transactionGuid = t.TransactionId;

                    Transaction transaction = new Transaction { TransactionGuid = transactionGuid };

                    await _db.Transactions.AddAsync(transaction);
                    await _db.SaveChangesAsync();

                    Event thisEvent = await _db.Events.SingleAsync(e => e.Id == id);
                    UserEvent thisUserEvent = await _db.UserEvents.SingleAsync(e => e.EventId == id && e.UserId == userId);

                    await _db.UserEventHistory.AddAsync(new UserEventHistory {
                        EventId = thisEvent.Id,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                        TransactionId = transaction.TransactionId,
                        ActionType = DatabaseActionTypes.Delete,
                        UserId = userId
                    });

                    await _db.SaveChangesAsync();

                    t.Commit();
                }catch(Exception ex) {
                    ModelState.AddModelError("", ex.ToString());
                    t.Rollback();
                }
            }

            return RedirectToAction("User", "Index");
        }

        [HttpGet]
        public async Task<IActionResult> EventStatuses() {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.EventStatuses);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            List<EventStatus> model = _db.EventStatuses.ToList();
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
                if (!_db.EventStatuses.Any(s => s.NormalizedName == model.Name.ToUpper())) {
                    _db.EventStatuses.Add(new EventStatus {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                        RequestDate = DateTime.Now
                    });

                    _db.SaveChanges();
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

            EventStatus model = _db.EventStatuses.Single(s => s.Id == id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, EventStatus model) {

            AuthorizationResult result = await _authService.AuthorizeAsync(User, _controllerId, EventOperations.UpdateStatus);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                if (!_db.EventStatuses.Any(s => s.Id != id && s.NormalizedName == model.Name.ToUpper())) {
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _db.EventStatuses.Attach(model);
                    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    _db.SaveChanges();
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

            if(!_db.Events.Any(e => e.EventStatusId == id)) {
                EventStatus model = _db.EventStatuses.Single(e => e.Id == id);
                _db.EventStatuses.Remove(model);
                _db.SaveChanges();
            }
            return RedirectToAction("EventStatuses");
        }
    }
}
 