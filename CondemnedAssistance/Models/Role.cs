using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models {
    public class Role : TemplateTable {
    }

    public class RoleAccess : TrackingTemplate {
        public long Id { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int ControllerId { get; set; }
        public Controller Controller { get; set; }

        public int ActionId { get; set; }
        public Action Action { get; set; }

        public bool IsAllowed { get; set; }
    }
}
