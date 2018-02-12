using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("User", Schema = Schemas.User)]
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

    [Table("Status", Schema = Schemas.User)]
    public class Status : TemplateTable { 
    }

    [Table("Type", Schema = Schemas.User)]
    public class Type : TemplateTable {
    }

    [Table("UserRegister", Schema = Schemas.User)]
    public class UserRegister : TrackingTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RegisterId { get; set; }
        public Register Register { get; set; }
    }

    [Table("UserAddress", Schema = Schemas.User)]
    public class UserAddress  : TrackingTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }

    [Table("UserProfession", Schema = Schemas.App)]
    public class UserProfession : TrackingTemplate {
        [Key]
        public long Id { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public int UserId { get; set; }
        //public User User { get; set; }
    }

    [Table("UserRole", Schema = Schemas.User)]
    public class UserRole : TrackingTemplate {
        [Key]
        public long Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
                
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    [Table("UserHelp", Schema = Schemas.App)]
    public class UserHelp {
        public int Id { get; set; }

        public int HelpId { get; set; }
        public Help Help { get; set; }

        public int UserId { get; set; }
        //public User User { get; set; }
    }

    // ---------------- History tables ------------------------- //
    [Table("UserHistory", Schema = Schemas.History)]
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

    [Table("UserStaticInfoHistory", Schema = Schemas.History)]
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

    [Table("UserRoleHistory", Schema = Schemas.History)]
    public class UserRoleHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
    }

    [Table("UserRegisterHistory", Schema = Schemas.History)]
    public class UserRegisterHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public int RegisterId { get; set; }
    }

    [Table("UserAddressHistory", Schema = Schemas.History)]
    public class UserAddressHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }
    }

    [Table("UserProfessionHistory", Schema = Schemas.History)]
    public class UserProfessionHistory : HistoryTemplate {
        [Key]
        public long Id { get; set; }

        public int ProfessionId { get; set; }

        public int UserId { get; set; }
    }
}
