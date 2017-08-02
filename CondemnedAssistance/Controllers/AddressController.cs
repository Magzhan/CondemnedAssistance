using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles = "2, 3")]
    public class AddressController : Controller{

        private UserContext _db;

        public AddressController(UserContext context) {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index() {
            List<AddressModel> model = new List<AddressModel>();
            _db.Addresses.ToList().ForEach(a => {
                AddressLevel addressLevel = _db.AddressLevels.FirstOrDefault(l => l.Id ==a.AddressLevelId);
                model.Add(new AddressModel {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    AddressLevelId = a.AddressLevelId,
                    AddressLevels = new List<AddressLevelModel> { new AddressLevelModel { Id = addressLevel.Id, Name = addressLevel.Name, Description = addressLevel.Description} }
                });
            });

            model = model.OrderBy(a => a.AddressLevelId).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create(int levelId, int parentId, int childId) {
            AddressModel model = new AddressModel();
            Address parentAddress = _db.Addresses.FirstOrDefault(a => a.Id == parentId);
            List<AddressLevelModel> addressLevels = new List<AddressLevelModel>();
            _db.AddressLevels.ToList().ForEach(row => {
                addressLevels.Add(new AddressLevelModel {
                    Id = row.Id,
                    Name = row.Name,
                    Description = row.Description
                });
            });
            model.AddressLevels.AddRange(addressLevels);
            model.AddressLevelId = levelId;
            if(parentId > 0 && parentAddress != null) {
                model.AddressParent = parentAddress;
                model.AddressParentId = parentId;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(AddressModel model) {
            if (ModelState.IsValid) {
                if(!_db.Addresses.Any(a => a.NormalizedName == model.Name.ToUpper())) {
                    Address address = new Address {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        AddressLevelId = model.AddressLevelId,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };

                    _db.Addresses.Add(address);
                    _db.SaveChanges();

                    switch (model.AddressParentId) {
                        case 0:
                            break;
                        default:
                            _db.AddressHierarchies.Add(new AddressHierarchy { ParentAddressId = model.AddressParentId, ChildAddressId = address.Id });
                            _db.SaveChanges();
                            break;
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id) {
            if (_db.Addresses.Any(a => a.Id == id)) {
                Address address = _db.Addresses.First(a => a.Id == id);
                Address addressParent = null;
                AddressHierarchy parent = _db.AddressHierarchies.FirstOrDefault(h => h.ChildAddressId == id);
                if(parent != null) {
                    addressParent = _db.Addresses.First(a => a.Id == parent.ParentAddressId);
                }

                AddressModel model = new AddressModel {
                    Id = address.Id,
                    Name = address.Name,
                    Description = address.Description,
                    AddressLevelId = address.AddressLevelId,
                    AddressLevels = _db.AddressLevels.ToList().Select(r => new AddressLevelModel { Id = r.Id, Name = r.Name, Description = r.Description}).ToList(),
                    AddressParent = addressParent
                };
                return View(model);
            }
            ModelState.AddModelError("", "Not found");
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, AddressModel model) {
            if (ModelState.IsValid) {
                if (!_db.Addresses.Any(a => a.Id != id && a.NormalizedName == model.Name.ToUpper())) {
                    Address address = _db.Addresses.FirstOrDefault(a => a.Id == id);
                    address.Name = model.Name;
                    address.Description = model.Description;
                    address.NormalizedName = model.Name.ToUpper();
                    address.RequestDate = DateTime.Now;
                    address.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.AddressLevels.ToList().ForEach(r => { model.AddressLevels.Add(new AddressLevelModel { Id = r.Id, Name = r.Name, Description = r.Description }); });

                    _db.Addresses.Attach(address);
                    _db.Entry(address).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            if(_db.UserAddresses.Any(a => a.AddressId == id)) {
                ModelState.AddModelError("", "Has users so cannot be deleted");
                return RedirectToAction("Index");
            }
            if (_db.AddressHierarchies.Any(a => a.ParentAddressId == id)) {
                ModelState.AddModelError("", "Has users so cannot be deleted");
                return RedirectToAction("Index");
            }

            Address address = _db.Addresses.First(a => a.Id == id);
            AddressHierarchy[] addressParents = _db.AddressHierarchies.Where(a => a.ChildAddressId == id).ToArray();
            _db.AddressHierarchies.RemoveRange(addressParents);

            _db.Addresses.Remove(address);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddressLevels() {
            List<AddressLevelModel> models = new List<AddressLevelModel>();
            foreach(AddressLevel addressLevel in _db.AddressLevels.ToList()) {
                models.Add(new AddressLevelModel {
                    Id = addressLevel.Id,
                    Name = addressLevel.Name,
                    Description = addressLevel.Description
                });
            }
            return View(models);
        }

        [HttpGet]
        public IActionResult CreateLevel() {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLevel(AddressLevelModel model) {
            if (ModelState.IsValid) {
                if(!_db.AddressLevels.Any(ad => ad.NormalizedName == model.Name.ToUpper())) {
                    AddressLevel addressLevel = new AddressLevel {
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        Description = model.Description,
                        RequestDate = DateTime.Now,
                        RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name)
                    };
                    _db.AddressLevels.Add(addressLevel);
                    _db.SaveChanges();
                    return RedirectToAction("AddressLevels");
                }
                ModelState.AddModelError("", "Already exists");
            }
            return View();
        }

        [HttpGet]
        public IActionResult UpdateLevel(int id) {
            if(_db.AddressLevels.Any(ad => ad.Id == id)) {
                AddressLevel addressLevel = _db.AddressLevels.First(a => a.Id == id);
                AddressLevelModel model = new AddressLevelModel {
                    Id = addressLevel.Id,
                    Name = addressLevel.Name,
                    Description = addressLevel.Description
                };
                return View(model);
            }
            return RedirectToAction("AddressLevels");
        }

        [HttpPost]
        public IActionResult UpdateLevel(int id, AddressLevelModel model) {
            if (ModelState.IsValid) {
                if (_db.AddressLevels.Any(a => a.Id == id)) {
                    AddressLevel addressLevel = _db.AddressLevels.First(a => a.Id == id);
                    addressLevel.Name = model.Name;
                    addressLevel.NormalizedName = model.Name.ToUpper();
                    addressLevel.Description = model.Description;
                    addressLevel.RequestDate = DateTime.Now;
                    addressLevel.RequestUser = Convert.ToInt32(HttpContext.User.Identity.Name);

                    _db.AddressLevels.Attach(addressLevel);
                    _db.Entry(addressLevel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                    return View(model);
                }                
            }
            return RedirectToAction("AddressLevels");
        }

        [HttpGet]
        public IActionResult DeleteLevel(int id) {
            if (_db.Addresses.Any(a => a.AddressLevelId == id)) {
                ModelState.AddModelError("", "It still has binded elements");
                return RedirectToAction("AddressLevels");
            }
            AddressLevel addressLevel = _db.AddressLevels.First(a => a.Id == id);
            _db.AddressLevels.Remove(addressLevel);
            return RedirectToAction("AddressLevels");
        }

        [HttpGet]
        public IActionResult GetAddressList(int addressId) {            
            int[] childrenIds = _db.AddressHierarchies.Where(a => a.ParentAddressId == addressId).Select(a => a.ChildAddressId).ToArray();
            AddressModel[] children = _db.Addresses.Where(a => childrenIds.Contains(a.Id)).Select(a => new AddressModel { Id = a.Id, Name = a.Name, Description = a.Description }).ToArray();            
            return PartialView("_DropDownList", children);
        }
    }    
}
