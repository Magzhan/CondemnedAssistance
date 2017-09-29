
namespace CondemnedAssistance.Models {
    public class Vacancy : TemplateTable{
    }

    public class VacancyProfession {
        public long Id { get; set; }

        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }
    }
}
