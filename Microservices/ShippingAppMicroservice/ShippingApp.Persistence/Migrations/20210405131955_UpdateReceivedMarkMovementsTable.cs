using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateReceivedMarkMovementsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovementRequestId",
                table: "ReceivedMarkPrintings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_MovementRequestId",
                table: "ReceivedMarkPrintings",
                column: "MovementRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivedMarkPrintings_MovementRequests_MovementRequestId",
                table: "ReceivedMarkPrintings",
                column: "MovementRequestId",
                principalTable: "MovementRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivedMarkPrintings_MovementRequests_MovementRequestId",
                table: "ReceivedMarkPrintings");

            migrationBuilder.DropIndex(
                name: "IX_ReceivedMarkPrintings_MovementRequestId",
                table: "ReceivedMarkPrintings");

            migrationBuilder.DropColumn(
                name: "MovementRequestId",
                table: "ReceivedMarkPrintings");
        }
    }
}
