using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CondemnedAssistance.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.EnsureSchema(
                name: "hist");

            migrationBuilder.CreateTable(
                name: "AddressHierarchy",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildAddressId = table.Column<int>(type: "int", nullable: false),
                    ParentAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressLevel",
                schema: "user",
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
                    table.PrimaryKey("PK_AddressLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kato",
                schema: "user",
                columns: table => new
                {
                    SystemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AreaType = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    NameKaz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kato", x => x.SystemId);
                });

            migrationBuilder.CreateTable(
                name: "RegisterHierarchy",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildRegister = table.Column<int>(type: "int", nullable: false),
                    ParentRegister = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterLevel",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFirstAncestor = table.Column<bool>(type: "bit", nullable: false),
                    IsLastChild = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterLevelHierarchy",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildLevel = table.Column<int>(type: "int", nullable: false),
                    ParentLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterLevelHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "user",
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
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sms",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "user",
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
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "user",
                columns: table => new
                {
                    TransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                schema: "user",
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
                    table.PrimaryKey("PK_Type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLevelId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_AddressLevel_AddressLevelId",
                        column: x => x.AddressLevelId,
                        principalSchema: "user",
                        principalTable: "AddressLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Register",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterLevelId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Register", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Register_RegisterLevel_RegisterLevelId",
                        column: x => x.RegisterLevelId,
                        principalSchema: "user",
                        principalTable: "RegisterLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddressHistory",
                schema: "hist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddressHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddressHistory_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "user",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistory",
                schema: "hist",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_UserHistory_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "user",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRegisterHistory",
                schema: "hist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegisterHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRegisterHistory_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "user",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleHistory",
                schema: "hist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleHistory_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "user",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStaticInfoHistory",
                schema: "hist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainAddress = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserStatusId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    Xin = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStaticInfoHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStaticInfoHistory_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "user",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailExchange",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmailId = table.Column<long>(type: "bigint", nullable: false),
                    IsSuccessfullySent = table.Column<bool>(type: "bit", nullable: false),
                    ReceiverEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailExchange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailExchange_Email_EmailId",
                        column: x => x.EmailId,
                        principalSchema: "user",
                        principalTable: "Email",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailExchange_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmailExchange_User_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SmsExchange",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsSuccessfullySent = table.Column<bool>(type: "bit", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    ReceiverPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SmsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsExchange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsExchange_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsExchange_User_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SmsExchange_Sms_SmsId",
                        column: x => x.SmsId,
                        principalSchema: "user",
                        principalTable: "Sms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "user",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStaticInfo",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainAddress = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserStatusId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    Xin = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStaticInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStaticInfo_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStaticInfo_Status_UserStatusId",
                        column: x => x.UserStatusId,
                        principalSchema: "user",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStaticInfo_Type_UserTypeId",
                        column: x => x.UserTypeId,
                        principalSchema: "user",
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddress_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "user",
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddress_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRegister",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUser = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRegister_Register_RegisterId",
                        column: x => x.RegisterId,
                        principalSchema: "user",
                        principalTable: "Register",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRegister_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAddressHistory_TransactionId",
                schema: "hist",
                table: "UserAddressHistory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_TransactionId",
                schema: "hist",
                table: "UserHistory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegisterHistory_TransactionId",
                schema: "hist",
                table: "UserRegisterHistory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleHistory_TransactionId",
                schema: "hist",
                table: "UserRoleHistory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfoHistory_TransactionId",
                schema: "hist",
                table: "UserStaticInfoHistory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_AddressLevelId",
                schema: "user",
                table: "Address",
                column: "AddressLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailExchange_EmailId",
                schema: "user",
                table: "EmailExchange",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailExchange_ReceiverId",
                schema: "user",
                table: "EmailExchange",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailExchange_SenderId",
                schema: "user",
                table: "EmailExchange",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Register_RegisterLevelId",
                schema: "user",
                table: "Register",
                column: "RegisterLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsExchange_ReceiverId",
                schema: "user",
                table: "SmsExchange",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsExchange_SenderId",
                schema: "user",
                table: "SmsExchange",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsExchange_SmsId",
                schema: "user",
                table: "SmsExchange",
                column: "SmsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_AddressId",
                schema: "user",
                table: "UserAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UserId",
                schema: "user",
                table: "UserAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegister_RegisterId",
                schema: "user",
                table: "UserRegister",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegister_UserId",
                schema: "user",
                table: "UserRegister",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "user",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                schema: "user",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfo_UserId",
                schema: "user",
                table: "UserStaticInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfo_UserStatusId",
                schema: "user",
                table: "UserStaticInfo",
                column: "UserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaticInfo_UserTypeId",
                schema: "user",
                table: "UserStaticInfo",
                column: "UserTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAddressHistory",
                schema: "hist");

            migrationBuilder.DropTable(
                name: "UserHistory",
                schema: "hist");

            migrationBuilder.DropTable(
                name: "UserRegisterHistory",
                schema: "hist");

            migrationBuilder.DropTable(
                name: "UserRoleHistory",
                schema: "hist");

            migrationBuilder.DropTable(
                name: "UserStaticInfoHistory",
                schema: "hist");

            migrationBuilder.DropTable(
                name: "AddressHierarchy",
                schema: "user");

            migrationBuilder.DropTable(
                name: "EmailExchange",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Kato",
                schema: "user");

            migrationBuilder.DropTable(
                name: "RegisterHierarchy",
                schema: "user");

            migrationBuilder.DropTable(
                name: "RegisterLevelHierarchy",
                schema: "user");

            migrationBuilder.DropTable(
                name: "SmsExchange",
                schema: "user");

            migrationBuilder.DropTable(
                name: "UserAddress",
                schema: "user");

            migrationBuilder.DropTable(
                name: "UserRegister",
                schema: "user");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "user");

            migrationBuilder.DropTable(
                name: "UserStaticInfo",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Email",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Sms",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Register",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "user");

            migrationBuilder.DropTable(
                name: "User",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Type",
                schema: "user");

            migrationBuilder.DropTable(
                name: "AddressLevel",
                schema: "user");

            migrationBuilder.DropTable(
                name: "RegisterLevel",
                schema: "user");
        }
    }
}
