using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CondemnedAssistance.Migrations
{
    public partial class UserStaticInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStaticInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    Gender = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    RequestUser = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserStatusId = table.Column<int>(nullable: false),
                    UserTypeId = table.Column<int>(nullable: false),
                    Xin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStaticInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStaticInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStaticInfo_UserStatuses_UserStatusId",
                        column: x => x.UserStatusId,
                        principalTable: "UserStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStaticInfo_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfo_UserId",
                table: "UserStaticInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfo_UserStatusId",
                table: "UserStaticInfo",
                column: "UserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfo_UserTypeId",
                table: "UserStaticInfo",
                column: "UserTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStaticInfo");
        }
    }
}
