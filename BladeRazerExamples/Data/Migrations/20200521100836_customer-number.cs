using Microsoft.EntityFrameworkCore.Migrations;

namespace BladeRazerExamples.Data.Migrations
{
    public partial class customernumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerNumber",
                table: "Customers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "Customers");
        }
    }
}
