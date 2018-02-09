using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    [Table("Education", Schema = "app")]
    public class Education : TemplateTable{
        public int EducationLevelId { get; set; }

        public EducationLevel EducationLevel { get; set; }
    }

    [Table("EducationLevel", Schema = "app")]
    public class EducationLevel : TemplateTable{
        ICollection<Education> Educations { get; set; }

        public EducationLevel() {
            Educations = new List<Education>();
        }
    }

    [Table("UserEducation", Schema = "app")]
    public class UserEducation {
        [Key]
        public int Id { get; set; }

        public int EducationId { get; set; }
        public Education Education { get; set; }

        public int UserId { get; set; }
    }
}
