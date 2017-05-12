using CondemnedAssistance.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CondemnedAssistance.ViewModels {
    public class UserModel : UserModelTemplate{
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public int UserStatusId { get; set; }
        public string UserStatusName { get; set; }

        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
    }

    public class UserModelModify : UserModelTemplate {
        public int RoleId { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public int UserStatusId { get; set; }
        public IEnumerable<UserStatus> UserStatuses { get; set; }
        public int UserTypeId { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
    }

    public class UserModelTemplate {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Xin { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
    }
}
