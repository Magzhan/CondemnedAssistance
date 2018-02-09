using CondemnedAssistance.Services.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    [Table("Register", Schema = Schemas.User)]
    public class Register : TemplateTable{
        public int RegisterLevelId { get; set; }
        public RegisterLevel RegisterLevel { get; set; }
    }

    [Table("RegisterHierarchy", Schema = Schemas.User)]
    public class RegisterHierarchy {
        public long Id { get; set; }
        public int ParentRegister { get; set; }
        public int ChildRegister { get; set; }
    }

    [Table("RegisterLevel", Schema = Schemas.User)]
    public class RegisterLevel : TemplateTable{
        public bool IsLastChild { get; set; }
        public bool IsFirstAncestor { get; set; }
    }

    [Table("RegisterLevelHierarchy", Schema = Schemas.User)]
    public class RegisterLevelHierarchy {
        public long Id { get; set; }
        public int ParentLevel { get; set; }
        public int ChildLevel { get; set; }
    }
}
