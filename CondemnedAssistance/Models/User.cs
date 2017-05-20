using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models
{
    public class User {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        public int PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public int ModifiedUserId { get; set; }
        public DateTimeOffset ModifiedUserDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public User() {
            UserRoles = new List<UserRole>();
        }
    }

    public class TemplateHelperTable {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public int RequestUser { get; set; }
        public DateTime RequestDate { get; set; }
    }

    public class UserStatus : TemplateHelperTable {

    }

    public class UserType : TemplateHelperTable {

    }

    public class UserRegister {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RegisterId { get; set; }
        public Register Register { get; set; }
    }
}
