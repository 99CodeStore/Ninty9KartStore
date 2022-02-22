using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NintyNineKartStore.Data.Migrations
{
    public partial class AddEmailIntoOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrderHeader");
        }
    }
}
