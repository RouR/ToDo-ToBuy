using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToBuyService.Migrations
{
    public partial class TobuyItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tobuy",
                columns: table => new
                {
                    PublicId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Qty = table.Column<float>(nullable: false),
                    Price_Amount = table.Column<decimal>(nullable: false),
                    Price_Currency = table.Column<int>(nullable: false),
                    DueToUtc = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tobuy", x => x.PublicId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tobuy_UserId_IsDeleted",
                table: "Tobuy",
                columns: new[] { "UserId", "IsDeleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tobuy");
        }
    }
}
