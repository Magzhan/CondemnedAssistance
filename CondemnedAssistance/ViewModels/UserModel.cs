using CondemnedAssistance.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Роль")]
        public int RoleId { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        [Display(Name = "Статус")]
        public int UserStatusId { get; set; }
        public IEnumerable<UserStatus> UserStatuses { get; set; }
        [Display(Name = "Тип пользователя")]
        public int UserTypeId { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
        [Display(Name = "Регистр")]
        public int UserRegisterId { get; set; }
        public IEnumerable<Register> UserRegisters { get; set; }
    }

    public class UserModelCreate : UserModelModify {
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Е-mail")]
        public string Email { get; set; }
        [Display(Name = "Адрес")]
        public string MainAddress { get; set; }
        [Display(Name = "Республика")]
        public int AddressLevelOneId { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        [Display(Name = "Область")]
        public int AddressLevelTwoId { get; set; }
        [Display(Name = "Район")]
        public int AddressLevelThreeId { get; set; }
    }

    public class UserModelTemplate {
        public int UserId { get; set; }
        [StringLength(12, ErrorMessage = "Длина символов не может превышать или быть меньше чем 12", MinimumLength = 12)]
        [Display(Name = "Логин")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Только числа могут быть логином")]
        public string Login { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        [Display(Name = "ИИН")]
        public string Xin { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime Birthdate { get; set; }
        [Display(Name = "Пол")]
        public bool Gender { get; set; }
    }

    public class UserProfileModel : UserModelTemplate{
        [Display(Name = "Мобильный телефон")]
        public string MobilePhone { get; set; }
        public bool IsMobileConfirmed { get; set; }
        public string IsMobileConfirmedText { get => (IsMobileConfirmed) ? "Yes" : "No"; }
        [Display(Name = "Е-mail")]
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string IsEmailConfirmedText { get => (IsEmailConfirmed) ? "Yes" : "No"; }
        [Display(Name = "Роль")]
        public string Role { get; set; }
        [Display(Name = "Статус")]
        public string Status { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Пол")]
        public string GenderText { get { return (base.Gender) ? "Male" : "Female"; } }
        [Display(Name = "Регистр")]
        public string Registration { get; set; }
        public IEnumerable<Profession> Professions { get; set; }
        public IEnumerable<Education> Educations { get; set; }
    }

    public class ChangePasswordModel {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
