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
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public ProfessionController(UserContext context, IAuthorizationService authorizationService) {
            _db = context;
            _authorizationService = authorizationService;
            _controllerId = _db.Controllers.Single(c => c.NormalizedName == Constants.Profession.ToUpper()).Id;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View(_db.Professions.ToList());
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
                if (!_db.Professions.Any(p => p.NormalizedName == model.Name.ToUpper())){
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _db.Professions.Add(model);
                    _db.SaveChanges();
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
            Profession model = _db.Professions.First(p => p.Id == id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Profession model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, ProfessionOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                if(!_db.Professions.Any(p => p.Id != id && p.NormalizedName == model.Name.ToUpper())) {
                    model.NormalizedName = model.Name.ToUpper();
                    model.RequestDate = DateTime.Now;
                    model.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Professions.Attach(model);
                    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
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
