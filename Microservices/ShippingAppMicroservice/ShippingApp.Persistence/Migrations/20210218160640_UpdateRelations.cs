using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovementRequestDetails",
                table: "MovementRequestDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "MovementRequestDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovementRequestDetails",
                table: "MovementRequestDetails",
                columns: new[] { "WorkOrderId", "MovementRequestId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_MovementRequestDetails_ProductId",
                table: "MovementRequestDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovementRequestDetails_Products_ProductId",
                table: "MovementRequestDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovementRequestDetails_Products_ProductId",
                table: "MovementRequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovementRequestDetails",
                table: "MovementRequestDetails");

            migrationBuilder.DropIndex(
                name: "IX_MovementRequestDetails_ProductId",
                table: "MovementRequestDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "MovementRequestDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovementRequestDetails",
                table: "MovementRequestDetails",
                columns: new[] { "WorkOrderId", "MovementRequestId" });
        }
    }
}
