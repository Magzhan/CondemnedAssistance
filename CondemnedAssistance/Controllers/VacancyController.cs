using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class VacancyController : Microsoft.AspNetCore.Mvc.Controller  {

        private UserContext _db;
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public VacancyController(UserContext context, IAuthorizationService authorizationService) {
            _db = context;
            _authorizationService = authorizationService;
            _controllerId = _db.Controllers.Single(c => c.NormalizedName == Constants.Vacancy.ToUpper()).Id;
        }
    }
}
