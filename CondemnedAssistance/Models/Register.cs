using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Register : TemplateTable{
        public int RegisterLevelId { get; set; }
        public RegisterLevel RegisterLevel { get; set; }
    }

    public class RegisterHierarchy {
        public long Id { get; set; }
        public int ParentRegister { get; set; }
        public int ChildRegister { get; set; }
    }

    public class RegisterLevel : TemplateTable{
        public bool IsLastChild { get; set; }
        public bool IsFirstAncestor { get; set; }
    }

    public class RegisterLevelHierarchy {
        public long Id { get; set; }
        public int ParentLevel { get; set; }
        public int ChildLevel { get; set; }
    }
}
