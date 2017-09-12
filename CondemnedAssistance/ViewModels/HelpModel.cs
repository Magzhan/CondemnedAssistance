using CondemnedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewModels {
    public class HelpModel {
        public int UserId { get; set; }

        public int[] HelpIds { get; set; }

        public List<Help> Helps { get; set; }

        public HelpModel() {
            Helps = new List<Help>();
        }
    }
}
