using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Address : TemplateHelperTable{
        public int AddressLevelId { get; set; }
        public AddressLevel AddressLevel { get; set; }
    }

    public class AddressHierarchy {
        public int Id { get; set; }
        public int ParentAddressId { get; set; }
        public int ChildAddressId { get; set; }
    }

    public class AddressLevel : TemplateHelperTable {
    }
}
