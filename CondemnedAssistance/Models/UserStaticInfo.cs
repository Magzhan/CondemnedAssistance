using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class UserStaticInfo {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Xin { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int UserStatusId { get; set; }
        public UserStatus UserStatus { get; set; }

        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public int RequestUser { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
