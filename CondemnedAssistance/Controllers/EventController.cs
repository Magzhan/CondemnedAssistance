using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2,3")]
    public class EventController : Controller {

        private UserContext _db;
        private IAuthorizationService _authService;

        public EventController(UserContext context, IAuthorizationService authService) {
            _db = context;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int userId) {
            Dictionary<string, int> actions = new Dictionary<string, int>();
            actions.Add("childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId);

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            EventModel model = new EventModel();
            model.UserId = userId;

            await _db.UserEvents.Where(e => e.UserId == userId).ForEachAsync(row => {
                model.Events.Add(_db.Events.Single(r => r.Id == row.EventId));
            });

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int userId) {
            Dictionary<string, int> actions = new Dictionary<string, int>();
            actions.Add("childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId);

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            EventCreateModel model = new EventCreateModel();
            model.EventStatuses = _db.EventStatuses.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, EventCreateModel model) {

            Dictionary<string, int> actions = new Dictionary<string, int>();
            actions.Add("childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId);

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

                            _db.Transactions.Add(transaction);
                            _db.SaveChanges();

                            Event myEvent = new Event {
                                Name = model.Name,
                                NormalizedName = model.Name.ToUpper(),
                                Description = model.Description,
                                Date = model.Date,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                            };
                            _db.Events.Add(myEvent);
                            _db.SaveChanges();

                            _db.UserEvents.Add(new UserEvent {
                                EventId = myEvent.Id,
                                UserId = userId,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                            });

                            _db.UserEventHistory.Add(new UserEventHistory {
                                EventId = myEvent.Id,
                                UserId = userId,
                                TransactionId = transaction.TransactionId,
                                RequestDate = DateTime.Now,
                                RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name),
                                ActionType = DatabaseActionTypes.Insert
                            });
                            _db.SaveChanges();
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

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id){
            int userId = _db.UserEvents.Single(e => e.EventId == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int>();
            actions.Add("childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId);

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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Event model) {
            int userId = _db.UserEvents.Single(e => e.EventId == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int>();
            actions.Add("childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId);

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

                            _db.Transactions.Add(transaction);
                            _db.SaveChanges();

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

                            t.Commit();
                        }
                        catch (Exception ex) {
                            ModelState.AddModelError("", ex.ToString());
                            t.Rollback();
                        }
                    }
                }
                ModelState.AddModelError("", "Already has event with same date"); 
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            int userId = _db.UserEvents.Single(e => e.EventId == id).UserId;
            Dictionary<string, int> actions = new Dictionary<string, int>();
            actions.Add("childId", _db.UserRegisters.Single(r => r.UserId == userId).RegisterId);

            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, actions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            

            return RedirectToAction("User", "Index");
        }

        [HttpGet]
        public IActionResult EventStatuses() {
            return View();
        }

        [HttpGet]
        public IActionResult CreateStatus() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateStatus(EventStatus model) {
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
        public IActionResult UpdateStatus(int id){
            EventStatus model = _db.EventStatuses.Single(s => s.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, EventStatus model) {
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
        public IActionResult DeleteStatus(int id) {
            if(!_db.Events.Any(e => e.EventStatusId == id)) {
                EventStatus model = _db.EventStatuses.Single(e => e.Id == id);
                _db.EventStatuses.Remove(model);
                _db.SaveChanges();
            }
            return RedirectToAction("EventStatuses");
        }
    }
}
 