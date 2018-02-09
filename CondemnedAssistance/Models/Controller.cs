using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    [Table("Controller", Schema = "app")]
    public class Controller : TemplateTable {
    }

    [Table("Action", Schema = "app")]
    public class Action : TemplateTable {
        public int ControllerId { get; set; }
        public Controller Controller { get; set; }
    }
}
