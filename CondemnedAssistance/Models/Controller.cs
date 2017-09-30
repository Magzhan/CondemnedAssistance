using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Controller : TemplateTable {
    }

    public class Action : TemplateTable {
        public int ControllerId { get; set; }
        public Controller Controller { get; set; }
    }
}
