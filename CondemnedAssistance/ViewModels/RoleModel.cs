using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewModels {
    public class RoleModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RoleAccessModel {

        public int RoleId { get; set; }

        public string[] ControllerIds { get; set; }
        public IEnumerable<Models.Controller> Controllers { get; set; }

        public int[] ActionIds { get; set; }
        public IEnumerable<Models.Action> Actions { get; set; }
    }
}
