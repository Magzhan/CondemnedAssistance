using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CondemnedAssistance.Migrations
{
    public partial class RemoveRelationshipsSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleHistory_Roles_RoleId",
                table: "UserRoleHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleHistory_Users_UserId",
                table: "UserRoleHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleHistory_RoleId",
                table: "UserRoleHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleHistory_UserId",
                table: "UserRoleHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRoleHistory_RoleId",
                table: "UserRoleHistory",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleHistory_UserId",
                table: "UserRoleHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleHistory_Roles_RoleId",
                table: "UserRoleHistory",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleHistory_Users_UserId",
                table: "UserRoleHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
