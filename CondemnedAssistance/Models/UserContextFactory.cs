using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext> {
        public UserContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CondemnedAssistance_1;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new UserContext(optionsBuilder.Options);
        }
    }
}
