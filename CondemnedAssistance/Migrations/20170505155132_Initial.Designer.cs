using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CondemnedAssistance.Models;

namespace CondemnedAssistance.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170505155132_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
        }
    }
}
