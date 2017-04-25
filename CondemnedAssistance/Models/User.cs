using System;

namespace CondemnedAssistance.Models {
    public class User {
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
    }
}
