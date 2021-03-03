using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShippingMarkReceived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippingMarkReceived",
                columns: table => new
                {
                    ShippingMarkId = table.Column<int>(nullable: false),
                    ReceivedMarkId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingMarkReceived");
        }
    }
}
