using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Role", Schema = "user")]
    public class Role : TemplateTable {
    }

    [Table("RoleAccess", Schema = "app")]
    public class RoleAccess : TrackingTemplate {
        public long Id { get; set; }

        public int RoleId { get; set; }

        public int ControllerId { get; set; }
        public Controller Controller { get; set; }

        public int ActionId { get; set; }
        public Action Action { get; set; }

        public bool IsAllowed { get; set; }
    }
}
