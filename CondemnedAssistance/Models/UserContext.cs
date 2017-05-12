using Microsoft.EntityFrameworkCore;

namespace CondemnedAssistance.Models {
    public class UserContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<UserStaticInfo> UserStaticInfo { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public UserContext(DbContextOptions options) : base(options) {

        }
    }
}
