﻿using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("UserStaticInfo", Schema = Schemas.User)]
    public class UserStaticInfo : TrackingTemplate{
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
        public User User { get; set; }

        public int UserStatusId { get; set; }
        public Status UserStatus { get; set; }

        public int UserTypeId { get; set; }
        public Type UserType { get; set; }
    }
}
