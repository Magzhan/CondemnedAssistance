using CondemnedAssistance.Services.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    [Table("Address", Schema = Schemas.User)]
    public class Address : TemplateTable{
        public int AddressLevelId { get; set; }
        public AddressLevel AddressLevel { get; set; }
    }

    [Table("AddressHierarchy", Schema = Schemas.User)]
    public class AddressHierarchy {
        public long Id { get; set; }
        public int ParentAddressId { get; set; }
        public int ChildAddressId { get; set; }
    }

    [Table("AddressLevel", Schema = Schemas.User)]
    public class AddressLevel : TemplateTable {
    }

    [Table("Kato", Schema = Schemas.User)]
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
