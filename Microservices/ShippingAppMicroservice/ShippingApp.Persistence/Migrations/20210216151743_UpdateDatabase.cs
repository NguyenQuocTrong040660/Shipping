using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartonNumber",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "PrintBy",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "PrintDate",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "PrintBy",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "PrintDate",
                table: "ReceivedMarks");

            migrationBuilder.AddColumn<int>(
                name: "PrintCount",
                table: "ShippingMarks",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "ShippingRequestId",
                table: "ShippingMarks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MovementRequestId",
                table: "ReceivedMarks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrintCount",
                table: "ReceivedMarks",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarks_ShippingRequestId",
                table: "ShippingMarks",
                column: "ShippingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarks_MovementRequestId",
                table: "ReceivedMarks",
                column: "MovementRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivedMarks_MovementRequests_MovementRequestId",
                table: "ReceivedMarks",
                column: "MovementRequestId",
                principalTable: "MovementRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingMarks_ShippingRequests_ShippingRequestId",
                table: "ShippingMarks",
                column: "ShippingRequestId",
                principalTable: "ShippingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivedMarks_MovementRequests_MovementRequestId",
                table: "ReceivedMarks");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingMarks_ShippingRequests_ShippingRequestId",
                table: "ShippingMarks");

            migrationBuilder.DropIndex(
                name: "IX_ShippingMarks_ShippingRequestId",
                table: "ShippingMarks");

            migrationBuilder.DropIndex(
                name: "IX_ReceivedMarks_MovementRequestId",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "PrintCount",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "ShippingRequestId",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "MovementRequestId",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "PrintCount",
                table: "ReceivedMarks");

            migrationBuilder.AddColumn<string>(
                name: "CartonNumber",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrintBy",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrintDate",
                table: "ShippingMarks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrintBy",
                table: "ReceivedMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrintDate",
                table: "ReceivedMarks",
                type: "datetime2",
                nullable: true);
        }
    }
}
