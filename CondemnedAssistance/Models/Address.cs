using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Address : TemplateHelperTable{
        public int AddressLevelId { get; set; }
        public AddressLevel AddressLevel { get; set; }
    }

    public class AddressHierarchy {
        public long Id { get; set; }
        public int ParentAddressId { get; set; }
        public int ChildAddressId { get; set; }
    }

    public class AddressLevel : TemplateHelperTable {
    }

    public class Kato {
        [Key]
        public int SystemId { get; set; }
        public int Id { get; set; }
        public int? Parent { get; set; }
        public int AreaType { get; set; }
        public int Level { get; set; }
        public string Code { get; set; }
        public string NameKaz { get; set; }
        public string NameRus { get; set; }
    }
}
