﻿// <auto-generated />
using CondemnedAssistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CondemnedAssistance.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CondemnedAssistance.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressLevelId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.HasIndex("AddressLevelId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.AddressHierarchy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChildAddressId");

                    b.Property<int>("ParentAddressId");

                    b.HasKey("Id");

                    b.ToTable("AddressHierarchies");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.AddressLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("AddressLevels");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Education", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("EducationLevelId");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.EducationLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("EducationLevels");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("EventStatusId");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.HasIndex("EventStatusId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.EventStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("EventStatuses");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Help", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("Helps");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Kato", b =>
                {
                    b.Property<int>("SystemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AreaType");

                    b.Property<string>("Code");

                    b.Property<int>("Id");

                    b.Property<int>("Level");

                    b.Property<string>("NameKaz");

                    b.Property<string>("NameRus");

                    b.Property<int?>("Parent");

                    b.HasKey("SystemId");

                    b.ToTable("Katos");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsRead");

                    b.Property<bool>("IsReceived");

                    b.Property<bool>("IsSent");

                    b.Property<DateTime>("ReadDate");

                    b.Property<DateTime>("ReceivedDate");

                    b.Property<int>("SenderId");

                    b.Property<DateTime>("SentDate");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.MessageExchange", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HelpId");

                    b.Property<long>("MessageId");

                    b.Property<int>("ReceiverId");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("HelpId");

                    b.HasIndex("MessageId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("MessageExchanges");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Profession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Register", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<int>("RegisterLevelId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.HasIndex("RegisterLevelId");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.RegisterHierarchy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChildRegister");

                    b.Property<int>("ParentRegister");

                    b.HasKey("Id");

                    b.ToTable("RegisterHierarchies");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.RegisterLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("RegisterLevels");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("TransactionGuid");

                    b.HasKey("TransactionId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTime>("LockoutEnd");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(11);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserAddress", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddresses");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserAddressHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionType");

                    b.Property<int>("AddressId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<long>("TransactionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserAddressHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserEducation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EducationId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EducationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEducations");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEvents");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserEventHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionType");

                    b.Property<int>("EventId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<long>("TransactionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserEventHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserHistory", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("ActionType");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("Id");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTime>("LockoutEnd");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(11);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<long>("TransactionId");

                    b.HasKey("RecordId");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserProfession", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProfessionId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProfessions");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserProfessionHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionType");

                    b.Property<int>("ProfessionId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<long>("TransactionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserProfessionHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRegister", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RegisterId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RegisterId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRegisters");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRegisterHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionType");

                    b.Property<int>("RegisterId");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<long>("TransactionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserRegisterHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRoleHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionType");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("RoleId");

                    b.Property<long>("TransactionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserRoleHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserStaticInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("FirstName");

                    b.Property<bool>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("MainAddress")
                        .HasMaxLength(2000);

                    b.Property<string>("MiddleName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("UserId");

                    b.Property<int>("UserStatusId");

                    b.Property<int>("UserTypeId");

                    b.Property<string>("Xin")
                        .HasMaxLength(12);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserStatusId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("UserStaticInfo");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserStaticInfoHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionType");

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("FirstName");

                    b.Property<bool>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("MainAddress")
                        .HasMaxLength(2000);

                    b.Property<string>("MiddleName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<long>("TransactionId");

                    b.Property<int>("UserId");

                    b.Property<int>("UserStatusId");

                    b.Property<int>("UserTypeId");

                    b.Property<string>("Xin")
                        .HasMaxLength(12);

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("UserStaticInfoHistory");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("UserStatuses");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.HasKey("Id");

                    b.ToTable("UserTypes");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Address", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.AddressLevel", "AddressLevel")
                        .WithMany()
                        .HasForeignKey("AddressLevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Education", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Event", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.EventStatus", "EventStatus")
                        .WithMany()
                        .HasForeignKey("EventStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Message", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.MessageExchange", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Help", "Help")
                        .WithMany()
                        .HasForeignKey("HelpId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Register", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.RegisterLevel", "RegisterLevel")
                        .WithMany()
                        .HasForeignKey("RegisterLevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserAddress", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserAddressHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserEducation", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Education", "Education")
                        .WithMany()
                        .HasForeignKey("EducationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserEvent", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserEventHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserProfession", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Profession", "Profession")
                        .WithMany()
                        .HasForeignKey("ProfessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserProfessionHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRegister", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Register", "Register")
                        .WithMany()
                        .HasForeignKey("RegisterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRegisterHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRoleHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserStaticInfo", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.UserStatus", "UserStatus")
                        .WithMany()
                        .HasForeignKey("UserStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserStaticInfoHistory", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
