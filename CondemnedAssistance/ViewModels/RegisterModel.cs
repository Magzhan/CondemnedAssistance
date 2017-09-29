using CondemnedAssistance.Models;
using System.Collections.Generic;

namespace CondemnedAssistance.ViewModels {
    public class RegisterModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int RegisterLevelId { get; set; }

        public List<RegisterLevelModel> RegisterLevels { get; set; }

        public List<RegisterLevelHierarchy> RegisterLevelHierarchies { get; set; }

        public int RegisterParentId { get; set; }

        public Register RegisterParent { get; set; }

        public RegisterModel(){
            RegisterLevels = new List<RegisterLevelModel>();
            RegisterLevelHierarchies = new List<RegisterLevelHierarchy>();
        }
    }

    public class RegisterLevelModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsFirstAncestor { get; set; }

        public bool IsLastChild { get; set; }

        public int ParentLevelId { get; set; }

        public List<RegisterLevel> RegisterLevels { get; set; }

        public RegisterLevelModel() {
            RegisterLevels = new List<RegisterLevel>();
        }
    }
}
