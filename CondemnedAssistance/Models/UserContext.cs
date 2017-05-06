using Microsoft.EntityFrameworkCore;

namespace CondemnedAssistance.Models {
    public class UserContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public UserContext(DbContextOptions options) : base(options) {

        }
    }
}
