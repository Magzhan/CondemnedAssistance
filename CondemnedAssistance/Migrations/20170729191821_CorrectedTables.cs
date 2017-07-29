using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CondemnedAssistance.Migrations
{
    public partial class CorrectedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentAdressId",
                table: "AddressHierarchies",
                newName: "ParentAddressId");

            migrationBuilder.AddColumn<string>(
                name: "MainAddress",
                table: "UserStaticInfo",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainAddress",
                table: "UserStaticInfo");

            migrationBuilder.RenameColumn(
                name: "ParentAddressId",
                table: "AddressHierarchies",
                newName: "ParentAdressId");
        }
    }
}
