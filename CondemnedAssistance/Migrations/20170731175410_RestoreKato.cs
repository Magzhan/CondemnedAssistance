using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CondemnedAssistance.Migrations
{
    public partial class RestoreKato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Katos",
                columns: table => new
                {
                    SystemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AreaType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    NameKaz = table.Column<string>(nullable: true),
                    NameRus = table.Column<string>(nullable: true),
                    Parent = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Katos", x => x.SystemId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Katos");
        }
    }
}
