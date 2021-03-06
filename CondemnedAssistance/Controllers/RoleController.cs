﻿using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Role;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class RoleController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private ApplicationContext _app;
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public RoleController(UserContext context, ApplicationContext app, IAuthorizationService authorizationService) {
            this._db = context;
            _app = app;
            _authorizationService = authorizationService;
            _controllerId = _app.Controllers.Single(c => c.NormalizedName == Constants.Role.ToUpper()).Id;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            ICollection<RoleModel> roles = new List<RoleModel>();
            
            foreach(var role in _db.Roles) {
                roles.Add(new RoleModel {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                });
            }            
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleModel model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == model.Name);
                if (role == null) {
                    _db.Roles.Add(new Role {
                        Name = model.Name,
                        Description = model.Description,
                        NormalizedName = model.Name.ToUpper(),
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    });
                    await _app.SaveChangesAsync();
                    return RedirectToAction("Index", "Role");
                } else {
                    ModelState.AddModelError("", "Following Role already exists");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Role role = _db.Roles.FirstOrDefault(r => r.Id == id);
            
            if (role == null) {
                ModelState.AddModelError("", "Role with following id does not exist");
                return RedirectToAction("Index", "Role");
            }
            RoleModel model = new RoleModel() {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RoleModel model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (role != null) {
                    role.Name = model.Name;
                    role.NormalizedName = model.Name.ToUpper();
                    role.Description = model.Description;
                    role.RequestDate = DateTime.Now;
                    role.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.Roles.Attach(role);
                    _db.Entry(role).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "Role");
                } else {
                    ModelState.AddModelError("", "User with current id does not exist");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.Delete);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            _db.Roles.Remove(role);
            await _app.SaveChangesAsync();
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        public async Task<IActionResult> RoleAccess(int id) {
            //AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.RoleAccess);

            //if (!result.Succeeded) {
            //    return new ChallengeResult();
            //}

            List<RoleAccess> roleAccess = await _app.RoleAccesses.Where(r => r.RoleId == id).ToListAsync();
            RoleAccessModel model = new RoleAccessModel();
            model.RoleId = id;
            model.ControllerIds = roleAccess.Select(r => r.ControllerId.ToString()).ToArray();
            model.ActionIds = roleAccess.Select(r => r.ActionId).ToArray();
            model.Controllers = await _app.Controllers.ToListAsync();
            model.Actions = await _app.Actions.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> RoleAccess(int id, RoleAccessModel model, string[] ControllerIds) {
            //AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RoleOperations.RoleAccess);

            //if (!result.Succeeded) {
            //    return new ChallengeResult();
            //}

            if (ModelState.IsValid) {
                List<RoleAccess> currAccess = await _app.RoleAccesses.Where(r => r.RoleId == model.RoleId).ToListAsync();
                if(currAccess.Count == 0) {
                    RoleAccess randomRoleAccess = await _app.RoleAccesses.FirstOrDefaultAsync();
                    currAccess = await _app.RoleAccesses.Where(r => r.RoleId == randomRoleAccess.RoleId).ToListAsync();

                    List<RoleAccess> newAccess = new List<RoleAccess>();

                    foreach(RoleAccess ra in currAccess) {
                        newAccess.Add(new RoleAccess { RoleId = model.RoleId, ControllerId = ra.ControllerId, ActionId = ra.ActionId, IsAllowed = ra.IsAllowed });
                    }

                    foreach(RoleAccess ra in newAccess) {
                        if (model.ControllerIds.Contains(ra.ControllerId.ToString()) && model.ActionIds.Contains(ra.ActionId)) {
                            ra.IsAllowed = true;
                        }
                    }

                    await _app.RoleAccesses.AddRangeAsync(newAccess);
                    await _app.SaveChangesAsync();
                }
                else {
                    currAccess.ForEach(row => { row.IsAllowed = false; });
                    currAccess.ForEach(row => {
                        if(model.ControllerIds.Contains(row.ControllerId.ToString()) && model.ActionIds.Contains(row.ActionId)) {
                            row.IsAllowed = true;
                        }
                    });

                    foreach(RoleAccess ra in currAccess) {
                        _app.RoleAccesses.Attach(ra);
                        _app.Entry(ra).State = EntityState.Modified;
                        await _app.SaveChangesAsync();
                    }
                }
            }

            model.Controllers = await _app.Controllers.ToListAsync();
            model.Actions = await _app.Actions.ToListAsync();

            //Lets checkout something

            return View(model);
        }
    }
}
