using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Address : TemplateUserHelperTables{
    }

    public class AddressHierarchy {
        public int Id { get; set; }

        public int ParentAddress { get; set; }

        public int ChildAddress { get; set; }
    }
}
