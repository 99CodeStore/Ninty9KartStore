using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NsdcTraingPartnerHub.Data.Migrations.NsdcTpDb
{
    public partial class CourseBatchChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BatchName",
                table: "TcCourseBatch",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TcCourseBatch",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TcCourseBatch_ApplicationUserId",
                table: "TcCourseBatch",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TcCourseBatch_AspNetUsers_ApplicationUserId",
                table: "TcCourseBatch",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TcCourseBatch_AspNetUsers_ApplicationUserId",
                table: "TcCourseBatch");

            migrationBuilder.DropIndex(
                name: "IX_TcCourseBatch_ApplicationUserId",
                table: "TcCourseBatch");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TcCourseBatch");

            migrationBuilder.AlterColumn<string>(
                name: "BatchName",
                table: "TcCourseBatch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
