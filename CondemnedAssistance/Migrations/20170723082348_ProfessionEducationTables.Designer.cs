using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CondemnedAssistance.Models;

namespace CondemnedAssistance.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170723082348_ProfessionEducationTables")]
    partial class ProfessionEducationTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CondemnedAssistance.Models.Education", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("EducationId");

                    b.Property<int?>("EducationLevelId");

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

                    b.Property<DateTime>("LockoutEnd");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<DateTime>("ModifiedUserDate");

                    b.Property<int>("ModifiedUserId");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("PasswordHash");

                    b.Property<int>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("RegistratedUserId");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.ToTable("Users");
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

            modelBuilder.Entity("CondemnedAssistance.Models.UserProfession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProfessionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProfessions");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRegister", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RegisterId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RegisterId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRegisters");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
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

                    b.Property<string>("Xin")
                        .HasMaxLength(12);

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

            modelBuilder.Entity("CondemnedAssistance.Models.Education", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.Register", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.RegisterLevel", "RegisterLevel")
                        .WithMany()
                        .HasForeignKey("RegisterLevelId")
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

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
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
        }
    }
}
