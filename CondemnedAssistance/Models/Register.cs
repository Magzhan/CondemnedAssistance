using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Register : TemplateHelperTable{
        public int RegisterLevelId { get; set; }
        public RegisterLevel RegisterLevel { get; set; }
    }

    public class RegisterHierarchy {
        public int Id { get; set; }

        public int ParentRegister { get; set; }

        public int ChildRegister { get; set; }
    }

    public class RegisterLevel : TemplateHelperTable{        
    }
}
