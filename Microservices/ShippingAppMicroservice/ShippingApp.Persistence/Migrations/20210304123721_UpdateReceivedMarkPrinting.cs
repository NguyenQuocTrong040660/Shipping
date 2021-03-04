using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateReceivedMarkPrinting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingMarkReceived");

            migrationBuilder.AddColumn<int>(
                name: "ShippingMarkId",
                table: "ReceivedMarkPrintings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_ShippingMarkId",
                table: "ReceivedMarkPrintings",
                column: "ShippingMarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivedMarkPrintings_ShippingMarks_ShippingMarkId",
                table: "ReceivedMarkPrintings",
                column: "ShippingMarkId",
                principalTable: "ShippingMarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivedMarkPrintings_ShippingMarks_ShippingMarkId",
                table: "ReceivedMarkPrintings");

            migrationBuilder.DropIndex(
                name: "IX_ReceivedMarkPrintings_ShippingMarkId",
                table: "ReceivedMarkPrintings");

            migrationBuilder.DropColumn(
                name: "ShippingMarkId",
                table: "ReceivedMarkPrintings");

            migrationBuilder.CreateTable(
                name: "ShippingMarkReceived",
                columns: table => new
                {
                    ShippingMarkId = table.Column<int>(type: "int", nullable: false),
                    ReceivedMarkId = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMarkReceived", x => new { x.ShippingMarkId, x.ReceivedMarkId });
                    table.ForeignKey(
                        name: "FK_ShippingMarkReceived_ReceivedMarks_ReceivedMarkId",
                        column: x => x.ReceivedMarkId,
                        principalTable: "ReceivedMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingMarkReceived_ShippingMarks_ShippingMarkId",
                        column: x => x.ShippingMarkId,
                        principalTable: "ShippingMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarkReceived_ReceivedMarkId",
                table: "ShippingMarkReceived",
                column: "ReceivedMarkId");
        }
    }
}
