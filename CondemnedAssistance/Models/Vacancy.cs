using CondemnedAssistance.Services.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Vacancy", Schema = Schemas.App)]
    public class Vacancy : TemplateTable{
    }

    [Table("VacancyProfession", Schema = Schemas.App)]
    public class VacancyProfession {
        public long Id { get; set; }

        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }
    }
}
