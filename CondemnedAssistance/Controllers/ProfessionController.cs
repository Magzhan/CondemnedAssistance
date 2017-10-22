using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Profession;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class ProfessionController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private ApplicationContext _app;
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public ProfessionController(UserContext context, ApplicationContext app, IAuthorizationService authorizationService) {
            _db = context;
            _app = app;
            _authorizationService = authorizationService;
            _controllerId = _app.Controllers.Single(c => c.NormalizedName == Constants.Profession.ToUpper()).Id;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View(_app.Professions.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Profession model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                if (!_app.Professions.Any(p => p.NormalizedName == model.Name.ToUpper())){
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _app.Professions.Add(model);
                    _app.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Profession model = _app.Professions.First(p => p.Id == id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Profession model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                if(!_app.Professions.Any(p => p.Id != id && p.NormalizedName == model.Name.ToUpper())) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _app.Professions.Attach(model);
                    _app.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _app.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Delete);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View();
        }
    }
}
