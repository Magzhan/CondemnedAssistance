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

        public int UserRegisterId { get; set; }
        public int UserRegister { get; set; }
    }

    public class UserModelModify : UserModelTemplate {
        public int RoleId { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public int UserStatusId { get; set; }
        public IEnumerable<UserStatus> UserStatuses { get; set; }
        public int UserTypeId { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
        public int UserRegisterId { get; set; }
        public IEnumerable<Register> UserRegisters { get; set; }
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

    public class UserProfileModel : UserModelTemplate{
        public string MobilePhone { get; set; }
        public bool IsMobileConfirmed { get; set; }
        public string IsMobileConfirmedText { get => (IsMobileConfirmed) ? "Yes" : "No"; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string IsEmailConfirmedText { get => (IsEmailConfirmed) ? "Yes" : "No"; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string GenderText { get { return (base.Gender) ? "Male" : "Female"; } }
        public string Registration { get; set; }
        public IEnumerable<Profession> Professeions { get; set; }
        public IEnumerable<Education> Educations { get; set; }
    }
}
