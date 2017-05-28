using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class RegisterController : Controller {
        private UserContext _db;
        private IAuthorizationService _authorizationService;

        public RegisterController(UserContext context, IAuthorizationService authorizationService) {
            _db = context;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult Index() {
            List<RegisterModel> registers = new List<RegisterModel>();
            _db.Registers.ToList().ForEach(r => {
                RegisterLevelModel registerLevelModel = new RegisterLevelModel();
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(row => row.Id == r.RegisterLevelId);
                registerLevelModel.Id = registerLevel.Id;
                registerLevelModel.Name = registerLevel.Name;
                registerLevelModel.Description = registerLevel.Description;
                registers.Add(new RegisterModel { Id = r.Id, Name = r.Name, Description = r.Description, RegisterLevelId = r.RegisterLevelId, RegisterLevels = new List<RegisterLevelModel> { registerLevelModel } });
            });
            return View(registers);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int levelId, int parentId, int childId) {
            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", levelId);
            registerActions.Add("parentId", parentId);
            registerActions.Add("childId", childId);
            if (!await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }

            Register parentRegister = _db.Registers.FirstOrDefault(r => r.Id == parentId);
            Register childRegister = _db.Registers.FirstOrDefault(r => r.Id == childId);
            RegisterModel model = new RegisterModel();
            List<RegisterLevelModel> registerLevels = new List<RegisterLevelModel>();
            _db.RegisterLevels.ToList().ForEach(row => {
                registerLevels.Add(new RegisterLevelModel { Id = row.Id, Name = row.Name, Description = row.Description });
            });
            model.RegisterLevels.AddRange(registerLevels);
            model.RegisterLevelId = levelId;
            if (parentId > 0 && parentRegister != null){
                model.RegisterParentId = parentId;
                model.RegisterParent = parentRegister;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterModel model) {
            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", model.RegisterLevelId);
            registerActions.Add("parentId", model.RegisterParentId);
            if (!await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                Register register = _db.Registers.FirstOrDefault(a => a.NormalizedName == model.Name.ToUpper());
                if (register == null) {
                    register = new Register();
                    register.Name = model.Name;
                    register.Description = model.Description;
                    register.NormalizedName = model.Name.ToUpper();
                    register.RegisterLevelId = model.RegisterLevelId;
                    register.RequestDate = DateTime.Now;
                    register.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);
                    _db.Registers.Add(register);
                    _db.SaveChanges();

                    switch (model.RegisterParentId) {
                        case 0:
                            break;
                        default:
                            _db.RegisterHierarchies.Add(new RegisterHierarchy { ChildRegister = register.Id, ParentRegister = model.RegisterParentId });
                            _db.SaveChanges();
                            break;
                    }
                } else {
                    ModelState.AddModelError("", "Such address already exists");
                }
            }
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        public IActionResult Update(int id) {
            Register register = _db.Registers.FirstOrDefault(r => r.Id == id);
                        
            RegisterModel model = new RegisterModel();
            List<RegisterLevel> registerLevels = _db.RegisterLevels.ToList();
            List<RegisterLevelModel> registerModels = new List<RegisterLevelModel>();
            if (register != null) {
                model.Id = register.Id;
                model.Name = register.Name;
                model.Description = register.Description;
                model.RegisterLevelId = register.RegisterLevelId;
                registerLevels.ForEach(row => registerModels.Add(new RegisterLevelModel { Id = row.Id, Name = row.Name, Description = row.Description }));
                model.RegisterLevels = registerModels;
                return View(model);
            }
            return RedirectToAction("Index", "Register");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, RegisterModel model) {
            Register register = _db.Registers.FirstOrDefault(r => r.Id == id);

            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", register.RegisterLevelId);
            registerActions.Add("parentId", model.RegisterParentId);
            if (!await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }

            if (ModelState.IsValid) {
                register.Name = model.Name;
                register.Description = model.Description;
                register.NormalizedName = model.Name.ToUpper();
                register.RequestDate = DateTime.Now;
                register.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                _db.RegisterLevels.ToList().ForEach(r => model.RegisterLevels.Add(new RegisterLevelModel { Id = r.Id, Name = r.Name, Description = r.Description }));

                _db.Registers.Attach(register);
                _db.Entry(register).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return View(model);
            }
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            RegisterHierarchy children = _db.RegisterHierarchies.FirstOrDefault(h => h.ParentRegister == id);
            Register register = _db.Registers.FirstOrDefault(r => r.Id == id);

            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", register.RegisterLevelId);
            if (!await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy")) {
                return new ChallengeResult();
            }

            if (children != null) {
                ModelState.AddModelError("", "Register has children so it cannot be deleted");
                return RedirectToAction("Index", "Register");
            }
            
            _db.Registers.Remove(register);
            _db.SaveChanges();
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        public IActionResult RegisterLevels() {
            List<RegisterLevelModel> registerLevels = new List<RegisterLevelModel>();
            foreach(var r in _db.RegisterLevels.ToList()) {
                registerLevels.Add(new RegisterLevelModel {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                });
            }
            return View(registerLevels);
        }

        [HttpGet]
        public IActionResult CreateRegisterLevel() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegisterLevel(RegisterLevelModel model) {
            if (ModelState.IsValid) {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.NormalizedName.Equals(model.Name.ToUpper()));
                if (registerLevel == null) {
                    registerLevel = new RegisterLevel {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };
                    _db.RegisterLevels.Add(registerLevel);
                    _db.SaveChanges();
                    return RedirectToAction("RegisterLevels", "Register");
                }
                else {
                    ModelState.AddModelError("", "Already exists");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateRegisterLevel(int id) {
            RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
            RegisterLevelModel model = null;
            if (registerLevel != null){
                model = new RegisterLevelModel {
                    Id = registerLevel.Id,
                    Name = registerLevel.Name,
                    Description = registerLevel.Description
                };
            }
            else {
                return RedirectToAction("RegisterLevels", "Register");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateRegisterLevel(int id, RegisterLevelModel model) {
            if (ModelState.IsValid) {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
                if (registerLevel != null) {
                    registerLevel.Name = model.Name;
                    registerLevel.NormalizedName = model.Name.ToUpper();
                    registerLevel.Description = model.Description;
                    registerLevel.RequestDate = DateTime.Now;
                    registerLevel.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.RegisterLevels.Attach(registerLevel);
                    _db.Entry(registerLevel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteRegisterLevel(int id) {
            if (_db.Registers.Count(r => r.RegisterLevelId == id) > 0) {
                ModelState.AddModelError("", "It still has binded elements");
                return RedirectToAction("RegisterLevels", "Register");
            }
            RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
            _db.RegisterLevels.Remove(registerLevel);
            _db.SaveChanges();
            
            return RedirectToAction("RegisterLevels", "Register");
        }
    }
}
