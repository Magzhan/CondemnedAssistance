using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    [Authorize(Roles="3")]
    public class VacancyController : Microsoft.AspNetCore.Mvc.Controller
    {
        
    }
}
