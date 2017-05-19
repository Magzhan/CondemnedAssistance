using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models {
    public class Role : TemplateHelperTable {
        public ICollection<UserRole> UserRoles { get; set; }

        public Role() {
            UserRoles = new List<UserRole>();
        }
    }
}
