using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class ApplicationContext : DbContext{

        public DbSet<Help> Helps { get; set; }
        public DbSet<UserHelp> UserHelps { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<UserProfession> UserProfessions { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<VacancyProfession> VacancyProfessions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<RoleAccess> RoleAccesses { get; set; }
        public DbSet<MessageExchange> MessageExchanges { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) {

        }
    }
}
