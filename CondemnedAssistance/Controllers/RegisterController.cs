using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Register;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class RegisterController : Microsoft.AspNetCore.Mvc.Controller {
        private UserContext _db;
        private IAuthorizationService _authorizationService;
        private RegisterHelper _registerHelper;
        private int _controllerId;

        public RegisterController(UserContext context, IAuthorizationService authorizationService) {
            _db = context;
            _authorizationService = authorizationService;
            _registerHelper = new RegisterHelper(context);
            _controllerId = _db.Controllers.Single(c => c.NormalizedName == Constants.Register.ToUpper()).Id;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            List<RegisterModel> registers = new List<RegisterModel>();
            if (User.IsInRole("3")) {
                _db.Registers.ToList().ForEach(r => {
                    RegisterLevelModel registerLevelModel = new RegisterLevelModel();
                    RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(row => row.Id == r.RegisterLevelId);
                    registerLevelModel.Id = registerLevel.Id;
                    registerLevelModel.Name = registerLevel.Name;
                    registerLevelModel.Description = registerLevel.Description;
                    registers.Add(new RegisterModel {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        RegisterLevelId = r.RegisterLevelId,
                        RegisterLevels = new List<RegisterLevelModel> { registerLevelModel },
                        RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList()
                    });
                });
            } else {
                int registerId = Convert.ToInt32(HttpContext.User.FindFirst(c => c.Type == "RegisterId").Value);
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                Register register = _db.Registers.First(r => r.Id == registerId);
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == register.RegisterLevelId);
                
                registers = _registerHelper.GetUserRegisterModels(userId, registerId);
                registers.Add(new RegisterModel {
                    Id = register.Id,
                    Name = register.Name,
                    Description = register.Description,
                    RegisterLevelId = register.RegisterLevelId,
                    RegisterLevels = new List<RegisterLevelModel> { new RegisterLevelModel { Id = registerLevel.Id, Name = registerLevel.Name, Description = registerLevel.Description } },
                    RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList()
                });

                registers = registers.OrderBy(r => r.RegisterLevelId).ToList();
            }            
            
            return View(registers);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int levelId, int parentId) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", levelId);
            registerActions.Add("parentId", parentId);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            Register parentRegister = _db.Registers.FirstOrDefault(r => r.Id == parentId);
            RegisterModel model = new RegisterModel();
            List<RegisterLevelModel> registerLevels = new List<RegisterLevelModel>();
            _db.RegisterLevels.ToList().ForEach(row => {
                registerLevels.Add(new RegisterLevelModel {
                    Id = row.Id,
                    Name = row.Name,
                    Description = row.Description
                });
            });
            model.RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList();
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
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.Create);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", model.RegisterLevelId);
            registerActions.Add("parentId", model.RegisterParentId);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
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
            model.RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList();
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Register register = _db.Registers.FirstOrDefault(r => r.Id == id);
            RegisterHierarchy registerHierarchy = _db.RegisterHierarchies.FirstOrDefault(r => r.ChildRegister == id);
            Register registerParent = null;
            if (registerHierarchy != null) {
                registerParent = _db.Registers.FirstOrDefault(r => r.Id == registerHierarchy.ParentRegister);
            }
            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", register.RegisterLevelId);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

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
                model.RegisterParent = registerParent;
                model.RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList();
                return View(model);
            }
            return RedirectToAction("Index", "Register");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, RegisterModel model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.Update);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Register register = _db.Registers.FirstOrDefault(r => r.Id == id);
            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", model.RegisterLevelId);
            registerActions.Add("parentId", model.RegisterParentId);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy");

            if (!authResult.Succeeded) {
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
                model.RegisterLevelHierarchies = _db.RegisterLevelHierarchies.ToList();
                return View(model);
            }
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.Delete);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            Register register = _db.Registers.FirstOrDefault(r => r.Id == id);

            Dictionary<string, int> registerActions = new Dictionary<string, int>();
            registerActions.Add("levelId", register.RegisterLevelId);
            int[] children = _registerHelper.GetRegisterChildren(new int[] { }, id);

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(User, registerActions, "resource-register-actions-policy");
            if (!authResult.Succeeded) {
                return new ChallengeResult();
            }

            if (_db.UserRegisters.Any(u => u.RegisterId == id)) {
                ModelState.AddModelError("", "Register has users so it cannot be deleted");
                return RedirectToAction("Index", "Register");
            }

            if (children != null) {
                ModelState.AddModelError("", "Register has children so it cannot be deleted");
                return RedirectToAction("Index", "Register");
            }

            var parents =  _db.RegisterHierarchies.Where(r => r.ChildRegister == id).ToArray();
            _db.RegisterHierarchies.RemoveRange(parents);

            _db.Registers.Remove(register);
            _db.SaveChanges();
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        public async Task<IActionResult> RegisterLevels() {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.RegisterLevels);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
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
        public async Task<IActionResult> CreateRegisterLevel(int parentId = 0) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.CreateLevel);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            RegisterLevelModel model = new RegisterLevelModel {
                IsFirstAncestor = parentId == 0 ? true:false,
                ParentLevelId = parentId
            };

            model.RegisterLevels = _db.RegisterLevels.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegisterLevel(RegisterLevelModel model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.CreateLevel);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.NormalizedName.Equals(model.Name.ToUpper()));
                if (registerLevel == null) {
                    registerLevel = new RegisterLevel {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        IsFirstAncestor = model.IsFirstAncestor,
                        IsLastChild = model.IsLastChild,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };
                    _db.RegisterLevels.Add(registerLevel);
                    _db.SaveChanges();

                    if (!model.IsFirstAncestor) {
                        RegisterLevelHierarchy registerLevelHierarchy = new RegisterLevelHierarchy {
                            ParentLevel = model.ParentLevelId,
                            ChildLevel = registerLevel.Id
                        };

                        _db.RegisterLevelHierarchies.Add(registerLevelHierarchy);
                        _db.SaveChanges();
                    }

                    return RedirectToAction("RegisterLevels", "Register");
                }
                else {
                    ModelState.AddModelError("", "Already exists");
                }
            }
            return  View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRegisterLevel(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.UpdateLevel);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
            RegisterLevelModel model = null;
            if (registerLevel != null){
                model = new RegisterLevelModel {
                    Id = registerLevel.Id,
                    Name = registerLevel.Name,
                    Description = registerLevel.Description,
                    IsFirstAncestor = registerLevel.IsFirstAncestor,
                    IsLastChild = registerLevel.IsLastChild
                };

                if (!registerLevel.IsFirstAncestor) {
                    model.ParentLevelId = _db.RegisterLevelHierarchies.Single(r => r.ChildLevel == registerLevel.Id).ParentLevel;
                }
            }
            else {
                return RedirectToAction("RegisterLevels", "Register");
            }
            model.RegisterLevels = _db.RegisterLevels.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRegisterLevel(int id, RegisterLevelModel model) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.UpdateLevel);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (ModelState.IsValid) {
                RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
                if (registerLevel != null) {
                    registerLevel.Name = model.Name;
                    registerLevel.NormalizedName = model.Name.ToUpper();
                    registerLevel.Description = model.Description;
                    registerLevel.IsFirstAncestor = model.IsFirstAncestor;
                    registerLevel.IsLastChild = model.IsLastChild;
                    registerLevel.RequestDate = DateTime.Now;
                    registerLevel.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.RegisterLevels.Attach(registerLevel);
                    _db.Entry(registerLevel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction("Register", "RegisterLevels");
                }
            }
            model.RegisterLevels = _db.RegisterLevels.ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRegisterLevel(int id) {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, RegisterOperations.DeleteLevel);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            if (_db.Registers.Count(r => r.RegisterLevelId == id) > 0 || _db.RegisterLevelHierarchies.Any(r => r.ParentLevel == id)) {
                ModelState.AddModelError("", "It still has binded elements");
                return RedirectToAction("RegisterLevels", "Register");
            }
            RegisterLevelHierarchy registerLevelHierarchy = _db.RegisterLevelHierarchies.Single(r => r.ChildLevel == id);
            _db.RegisterLevelHierarchies.Remove(registerLevelHierarchy);
            RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == id);
            _db.RegisterLevels.Remove(registerLevel);
            _db.SaveChanges();
            
            return RedirectToAction("RegisterLevels", "Register");
        }        
    }
}
