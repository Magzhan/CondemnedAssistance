using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CondemnedAssistance.Migrations
{
    public partial class ASmallFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_RegisterLevels_RegisterLevelId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Registers");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_RegisterLevelId",
                table: "Registers",
                newName: "IX_Registers_RegisterLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registers",
                table: "Registers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_RegisterLevels_RegisterLevelId",
                table: "Registers",
                column: "RegisterLevelId",
                principalTable: "RegisterLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registers_RegisterLevels_RegisterLevelId",
                table: "Registers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registers",
                table: "Registers");

            migrationBuilder.RenameTable(
                name: "Registers",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Registers_RegisterLevelId",
                table: "Addresses",
                newName: "IX_Addresses_RegisterLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_RegisterLevels_RegisterLevelId",
                table: "Addresses",
                column: "RegisterLevelId",
                principalTable: "RegisterLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
