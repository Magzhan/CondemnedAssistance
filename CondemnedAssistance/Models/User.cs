using System;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models {
    public class User : TrackingTemplate {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(12)]
        [MaxLength(12)]
        public string Login { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
    }

    public class Status : TemplateTable { 
    }

    public class Type : TemplateTable {
    }

    public class UserRegister : TrackingTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RegisterId { get; set; }
        public Register Register { get; set; }
    }

    public class UserAddress  : TrackingTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }

    public class UserProfession : TrackingTemplate {
        [Key]
        public long Id { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public int UserId { get; set; }
    }

    public class UserRole : TrackingTemplate {
        [Key]
        public long Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
                
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class UserHelp {
        public int Id { get; set; }

        public int HelpId { get; set; }
        public Help Help { get; set; }

        public int UserId { get; set; }
    }

    // ---------------- History tables ------------------------- //

    public class UserHistory : HistoryTemplate {
        [Key]
        public int RecordId { get; set; }
        public int Id { get; set; }
        [Required]
        [MinLength(12)]
        [MaxLength(12)]
        public string Login { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
    }

    public class UserStaticInfoHistory : HistoryTemplate {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [MaxLength(12)]
        [MinLength(12)]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "")]
        public string Xin { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        [MaxLength(2000)]
        public string MainAddress { get; set; }

        public int UserId { get; set; }

        public int UserStatusId { get; set; }

        public int UserTypeId { get; set; }
    }

    public class UserRoleHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
    }

    public class UserRegisterHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public int RegisterId { get; set; }
    }

    public class UserAddressHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }
    }

    public class UserProfessionHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int ProfessionId { get; set; }

        public int UserId { get; set; }
    }
}
