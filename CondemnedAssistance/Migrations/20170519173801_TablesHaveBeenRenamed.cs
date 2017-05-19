using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CondemnedAssistance.Migrations
{
    public partial class TablesHaveBeenRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressHierarchies");

            migrationBuilder.AddColumn<int>(
                name: "RegisterLevelId",
                table: "Addresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RegisterHierarchies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildRegister = table.Column<int>(nullable: false),
                    ParentRegister = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterHierarchies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    RequestUser = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterLevels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RegisterLevelId",
                table: "Addresses",
                column: "RegisterLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_RegisterLevels_RegisterLevelId",
                table: "Addresses",
                column: "RegisterLevelId",
                principalTable: "RegisterLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_RegisterLevels_RegisterLevelId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "RegisterHierarchies");

            migrationBuilder.DropTable(
                name: "RegisterLevels");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_RegisterLevelId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "RegisterLevelId",
                table: "Addresses");

            migrationBuilder.CreateTable(
                name: "AddressHierarchies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildAddress = table.Column<int>(nullable: false),
                    ParentAddress = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressHierarchies", x => x.Id);
                });
        }
    }
}
