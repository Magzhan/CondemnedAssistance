using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class RegisterViewComponent : ViewComponent {
        private readonly UserContext _db;

        public RegisterViewComponent(UserContext context) {
            _db = context;
        }

        public IViewComponentResult Invoke(int registerLevelId){
            RegisterLevel registerLevel = _db.RegisterLevels.FirstOrDefault(r => r.Id == registerLevelId);
            RegisterLevelModel registerLevelModel = new RegisterLevelModel { Id = registerLevel.Id, Name = registerLevel.Name };
            switch (registerLevelId) {
                case 1:                    
                    return View("FirstLevel", registerLevelModel);
                case 2:
                    Register parentRegister = _db.Registers.FirstOrDefault(r => r.RegisterLevelId == registerLevelId);
                    RegisterModel model = new RegisterModel();
                    model.RegisterLevelId = registerLevelId;
                    model.RegisterLevels.Add(registerLevelModel);
                    return View("SecondLevel", model);
                case 3:
                    break;
            }
            return View();
        }
    }
}
