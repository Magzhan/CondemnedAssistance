using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CondemnedAssistance.Migrations
{
    public partial class RemoveRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddressHistory_Addresses_AddressId",
                table: "UserAddressHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddressHistory_Users_UserId",
                table: "UserAddressHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfessionHistory_Professions_ProfessionId",
                table: "UserProfessionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfessionHistory_Users_UserId",
                table: "UserProfessionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRegisterHistory_Registers_RegisterId",
                table: "UserRegisterHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRegisterHistory_Users_UserId",
                table: "UserRegisterHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStaticInfoHistory_Users_UserId",
                table: "UserStaticInfoHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStaticInfoHistory_UserStatuses_UserStatusId",
                table: "UserStaticInfoHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStaticInfoHistory_UserTypes_UserTypeId",
                table: "UserStaticInfoHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserStaticInfoHistory_UserId",
                table: "UserStaticInfoHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserStaticInfoHistory_UserStatusId",
                table: "UserStaticInfoHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserStaticInfoHistory_UserTypeId",
                table: "UserStaticInfoHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserRegisterHistory_RegisterId",
                table: "UserRegisterHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserRegisterHistory_UserId",
                table: "UserRegisterHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserProfessionHistory_ProfessionId",
                table: "UserProfessionHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserProfessionHistory_UserId",
                table: "UserProfessionHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserAddressHistory_AddressId",
                table: "UserAddressHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserAddressHistory_UserId",
                table: "UserAddressHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfoHistory_UserId",
                table: "UserStaticInfoHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfoHistory_UserStatusId",
                table: "UserStaticInfoHistory",
                column: "UserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfoHistory_UserTypeId",
                table: "UserStaticInfoHistory",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegisterHistory_RegisterId",
                table: "UserRegisterHistory",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegisterHistory_UserId",
                table: "UserRegisterHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfessionHistory_ProfessionId",
                table: "UserProfessionHistory",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfessionHistory_UserId",
                table: "UserProfessionHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddressHistory_AddressId",
                table: "UserAddressHistory",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddressHistory_UserId",
                table: "UserAddressHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddressHistory_Addresses_AddressId",
                table: "UserAddressHistory",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddressHistory_Users_UserId",
                table: "UserAddressHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfessionHistory_Professions_ProfessionId",
                table: "UserProfessionHistory",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfessionHistory_Users_UserId",
                table: "UserProfessionHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRegisterHistory_Registers_RegisterId",
                table: "UserRegisterHistory",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRegisterHistory_Users_UserId",
                table: "UserRegisterHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStaticInfoHistory_Users_UserId",
                table: "UserStaticInfoHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStaticInfoHistory_UserStatuses_UserStatusId",
                table: "UserStaticInfoHistory",
                column: "UserStatusId",
                principalTable: "UserStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStaticInfoHistory_UserTypes_UserTypeId",
                table: "UserStaticInfoHistory",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
