using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NsdcTraingPartnerHub.Data.Migrations.NsdcTpDb
{
    public partial class StudentMissingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCenter_TrainingPartner_TrainingPartnerId",
                table: "TrainingCenter");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCenter_TrainingPartnerId",
                table: "TrainingCenter");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TrainingCenter",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Student",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedOn",
                table: "Student",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateIndex(
            //    name: "IX_TrainingCenter_TrainingPartnerId",
            //    table: "TrainingCenter",
            //    column: "TrainingPartnerId",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCenter_TrainingPartner_TrainingPartnerId",
                table: "TrainingCenter",
                column: "TrainingPartnerId",
                principalTable: "TrainingPartner",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCenter_TrainingPartner_TrainingPartnerId",
                table: "TrainingCenter");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCenter_TrainingPartnerId",
                table: "TrainingCenter");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "LastChangedOn",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TrainingCenter",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCenter_TrainingPartnerId",
                table: "TrainingCenter",
                column: "TrainingPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCenter_TrainingPartner_TrainingPartnerId",
                table: "TrainingCenter",
                column: "TrainingPartnerId",
                principalTable: "TrainingPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
