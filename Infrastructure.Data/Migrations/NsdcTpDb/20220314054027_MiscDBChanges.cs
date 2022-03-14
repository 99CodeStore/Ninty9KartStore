using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NsdcTraingPartnerHub.Data.Migrations.NsdcTpDb
{
    public partial class MiscDBChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_TraningPartner_TrainingPartnerId",
                table: "Course");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TrainingCenter_TraningPartner_TrainingPartnerId",
            //    table: "TrainingCenter");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_TraningPartner",
            //    table: "TraningPartner");

            //migrationBuilder.RenameTable(
            //    name: "TraningPartner",
            //    newName: "TrainingPartner");

            migrationBuilder.AlterColumn<string>(
                name: "CenterName",
                table: "TrainingCenter",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "TrainingPartnerId",
                table: "Course",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_TrainingPartner",
            //    table: "TrainingPartner",
            //    column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_TrainingPartner_TrainingPartnerId",
                table: "Course",
                column: "TrainingPartnerId",
                principalTable: "TrainingPartner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCenter_TrainingPartner_TrainingPartnerId",
                table: "TrainingCenter",
                column: "TrainingPartnerId",
                principalTable: "TrainingPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_TrainingPartner_TrainingPartnerId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCenter_TrainingPartner_TrainingPartnerId",
                table: "TrainingCenter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingPartner",
                table: "TrainingPartner");

            migrationBuilder.RenameTable(
                name: "TrainingPartner",
                newName: "TraningPartner");

            migrationBuilder.AlterColumn<string>(
                name: "CenterName",
                table: "TrainingCenter",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "TrainingPartnerId",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TraningPartner",
                table: "TraningPartner",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_TraningPartner_TrainingPartnerId",
                table: "Course",
                column: "TrainingPartnerId",
                principalTable: "TraningPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCenter_TraningPartner_TrainingPartnerId",
                table: "TrainingCenter",
                column: "TrainingPartnerId",
                principalTable: "TraningPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
