using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Education : TemplateTable{
        public int EducationLevelId { get; set; }

        public EducationLevel EducationLevel { get; set; }
    }

    public class EducationLevel : TemplateTable{
        ICollection<Education> Educations { get; set; }

        public EducationLevel() {
            Educations = new List<Education>();
        }
    }

    public class UserEducation {
        [Key]
        public int Id { get; set; }

        public int EducationId { get; set; }
        public Education Education { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
