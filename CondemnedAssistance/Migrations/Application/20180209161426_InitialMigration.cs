using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CondemnedAssistance.Migrations.Application
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Controller",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevel",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventStatus",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Help",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Help", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profession",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vacancy",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Controller_ControllerId",
                        column: x => x.ControllerId,
                        principalSchema: "app",
                        principalTable: "Controller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationLevelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_EducationLevel_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalSchema: "app",
                        principalTable: "EducationLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_EventStatus_EventStatusId",
                        column: x => x.EventStatusId,
                        principalSchema: "app",
                        principalTable: "EventStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHelp",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HelpId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHelp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHelp_Help_HelpId",
                        column: x => x.HelpId,
                        principalSchema: "app",
                        principalTable: "Help",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageExchange",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HelpId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageExchange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageExchange_Help_HelpId",
                        column: x => x.HelpId,
                        principalSchema: "app",
                        principalTable: "Help",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageExchange_Message_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "app",
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfession",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfessionId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfession_Profession_ProfessionId",
                        column: x => x.ProfessionId,
                        principalSchema: "app",
                        principalTable: "Profession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancyProfession",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfessionId = table.Column<int>(type: "int", nullable: false),
                    VacancyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyProfession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacancyProfession_Profession_ProfessionId",
                        column: x => x.ProfessionId,
                        principalSchema: "app",
                        principalTable: "Profession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancyProfession_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalSchema: "app",
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleAccess",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleAccess_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "app",
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RoleAccess_Controller_ControllerId",
                        column: x => x.ControllerId,
                        principalSchema: "app",
                        principalTable: "Controller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserEducation",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EducationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEducation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEducation_Education_EducationId",
                        column: x => x.EducationId,
                        principalSchema: "app",
                        principalTable: "Education",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ControllerId",
                schema: "app",
                table: "Action",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_EducationLevelId",
                schema: "app",
                table: "Education",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventStatusId",
                schema: "app",
                table: "Event",
                column: "EventStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageExchange_HelpId",
                schema: "app",
                table: "MessageExchange",
                column: "HelpId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageExchange_MessageId",
                schema: "app",
                table: "MessageExchange",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAccess_ActionId",
                schema: "app",
                table: "RoleAccess",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAccess_ControllerId",
                schema: "app",
                table: "RoleAccess",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducation_EducationId",
                schema: "app",
                table: "UserEducation",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHelp_HelpId",
                schema: "app",
                table: "UserHelp",
                column: "HelpId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfession_ProfessionId",
                schema: "app",
                table: "UserProfession",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyProfession_ProfessionId",
                schema: "app",
                table: "VacancyProfession",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyProfession_VacancyId",
                schema: "app",
                table: "VacancyProfession",
                column: "VacancyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event",
                schema: "app");

            migrationBuilder.DropTable(
                name: "MessageExchange",
                schema: "app");

            migrationBuilder.DropTable(
                name: "RoleAccess",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UserEducation",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UserHelp",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UserProfession",
                schema: "app");

            migrationBuilder.DropTable(
                name: "VacancyProfession",
                schema: "app");

            migrationBuilder.DropTable(
                name: "EventStatus",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Message",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Education",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Help",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Profession",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Vacancy",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Controller",
                schema: "app");

            migrationBuilder.DropTable(
                name: "EducationLevel",
                schema: "app");
        }
    }
}
