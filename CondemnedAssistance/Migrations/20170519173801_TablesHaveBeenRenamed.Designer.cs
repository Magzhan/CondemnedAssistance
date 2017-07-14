﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CondemnedAssistance.Models;

namespace CondemnedAssistance.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170519173801_TablesHaveBeenRenamed")]
    partial class TablesHaveBeenRenamed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.RegisterHierarchy", b =>
                {
                    b.Property<int>("Id")
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

            modelBuilder.Entity("CondemnedAssistance.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset>("LockoutEnd");

                    b.Property<string>("Login");

                    b.Property<DateTimeOffset>("ModifiedUserDate");

                    b.Property<int>("ModifiedUserId");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("PasswordHash");

                    b.Property<int>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("RoleId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserStaticInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("FirstName");

                    b.Property<bool>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<DateTime>("RequestDate");

                    b.Property<int>("RequestUser");

                    b.Property<int>("UserId");

                    b.Property<int>("UserStatusId");

                    b.Property<int>("UserTypeId");

                    b.Property<string>("Xin");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserStatusId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("UserStaticInfo");
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

            modelBuilder.Entity("CondemnedAssistance.Models.Register", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.RegisterLevel", "RegisterLevel")
                        .WithMany()
                        .HasForeignKey("RegisterLevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");
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
        }
    }
}