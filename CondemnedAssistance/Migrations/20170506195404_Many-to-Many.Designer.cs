using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CondemnedAssistance.Models;

namespace CondemnedAssistance.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170506195404_Many-to-Many")]
    partial class ManytoMany
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("CondemnedAssistance.Models.UserRole", b =>
                {
                    b.HasOne("CondemnedAssistance.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CondemnedAssistance.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
