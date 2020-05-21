using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BladeRazerExamples.Data.Migrations
{
    public partial class customerregistered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Registered",
                table: "Customers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Registered",
                table: "Customers");
        }
    }
}
