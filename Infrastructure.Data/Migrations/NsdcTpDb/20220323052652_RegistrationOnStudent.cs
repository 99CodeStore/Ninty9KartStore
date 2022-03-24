using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NsdcTraingPartnerHub.Data.Migrations.NsdcTpDb
{
    public partial class RegistrationOnStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_JobSector_JobSectorId",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNo",
                table: "Student",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobSectorId",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_JobSector_JobSectorId",
                table: "Course",
                column: "JobSectorId",
                principalTable: "JobSector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_JobSector_JobSectorId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "RegistrationNo",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "JobSectorId",
                table: "Course",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_JobSector_JobSectorId",
                table: "Course",
                column: "JobSectorId",
                principalTable: "JobSector",
                principalColumn: "Id");
        }
    }
}
