using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Migrations
{
    public partial class LoginAttempts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "Users",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SaltedPwd",
                table: "Users",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LoginAttempt",
                columns: table => new
                {
                    PublicId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 80, nullable: false),
                    IP = table.Column<string>(maxLength: 40, nullable: true),
                    AtUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempt", x => x.PublicId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempt_Email_AtUtc",
                table: "LoginAttempt",
                columns: new[] { "Email", "AtUtc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginAttempt");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SaltedPwd",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);
        }
    }
}
