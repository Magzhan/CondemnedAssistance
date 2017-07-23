using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Education : TemplateHelperTable{
        public int EducationId { get; set; }

        public EducationLevel EducationLevel { get; set; }
    }

    public class EducationLevel : TemplateHelperTable{
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
