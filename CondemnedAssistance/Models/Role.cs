using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models {
    public class Role {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public int RequestUser { get; set; }
        public DateTime RequestDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
