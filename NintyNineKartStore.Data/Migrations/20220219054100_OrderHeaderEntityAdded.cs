using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NintyNineKartStore.Data.Migrations
{
    public partial class OrderHeaderEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_ApplicationUser_ApplicationUserId",
                table: "ShoppingCart");



            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ShoppingCart",
                type: "float",
                nullable: false,
                defaultValue: 0.0);


            migrationBuilder.CreateTable(
                name: "OrderHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeader_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeader_ApplicationUserId",
                table: "OrderHeader",
                column: "ApplicationUserId");


            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_ApplicationUserId",
                table: "ShoppingCart",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_ApplicationUserId",
                table: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "OrderHeader");


            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCart");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "ApplicationUser");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_ApplicationUser_ApplicationUserId",
                table: "ShoppingCart",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
