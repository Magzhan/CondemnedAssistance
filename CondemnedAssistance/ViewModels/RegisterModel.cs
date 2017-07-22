using CondemnedAssistance.Models;
using System.Collections.Generic;

namespace CondemnedAssistance.ViewModels {
    public class RegisterModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int RegisterLevelId { get; set; }

        public List<RegisterLevelModel> RegisterLevels { get; set; }

        public int RegisterParentId { get; set; }

        public Register RegisterParent { get; set; }

        public RegisterModel(){
            RegisterLevels = new List<RegisterLevelModel>();
        }
    }

    public class RegisterLevelModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
