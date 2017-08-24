﻿using Microsoft.EntityFrameworkCore;

namespace CondemnedAssistance.Models {
    public class UserContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<UserStaticInfo> UserStaticInfo { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<RegisterHierarchy> RegisterHierarchies { get; set; }
        public DbSet<RegisterLevel> RegisterLevels { get; set; }
        public DbSet<UserRegister> UserRegisters { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Help> Helps { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<UserProfession> UserProfessions { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressLevel> AddressLevels { get; set; }
        public DbSet<AddressHierarchy> AddressHierarchies { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<Kato> Katos { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<UserHistory> UserHistory { get; set; }
        public DbSet<UserStaticInfoHistory> UserStaticInfoHistory { get; set; }
        public DbSet<UserRoleHistory> UserRoleHistory { get; set; }
        public DbSet<UserRegisterHistory> UserRegisterHistory { get; set; }
        public DbSet<UserAddressHistory> UserAddressHistory { get; set; }
        public DbSet<UserProfessionHistory> UserProfessionHistory { get; set; }
        public UserContext(DbContextOptions options) : base(options) {

        }
    }
}
