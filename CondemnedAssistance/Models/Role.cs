using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models {
    public class Role : TemplateUserHelperTables {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
