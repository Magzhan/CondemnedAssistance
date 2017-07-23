using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Profession : TemplateHelperTable{
    }

    public class UserProfession {
        [Key]
        public int Id { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
