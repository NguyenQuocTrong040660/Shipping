using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ProductNumber",
                table: "ShippingPlans");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippingDate",
                table: "ShippingPlans",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShippingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPlans_ProductId",
                table: "ShippingPlans",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingPlans_Products_ProductId",
                table: "ShippingPlans",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingPlans_Products_ProductId",
                table: "ShippingPlans");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPlans_ProductId",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShippingPlans");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingDate",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNumber",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
