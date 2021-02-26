using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRequestDetails_ShippingRequests_ShippingPlanId",
                table: "ShippingRequestDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestDetails_ShippingPlanId",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "ShippingPlanId",
                table: "ShippingRequestDetails");

            migrationBuilder.AddColumn<int>(
                name: "ShippingRequestId",
                table: "ShippingRequestDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestDetails_ShippingRequestId",
                table: "ShippingRequestDetails",
                column: "ShippingRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRequestDetails_ShippingRequests_ShippingRequestId",
                table: "ShippingRequestDetails",
                column: "ShippingRequestId",
                principalTable: "ShippingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRequestDetails_ShippingRequests_ShippingRequestId",
                table: "ShippingRequestDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestDetails_ShippingRequestId",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "ShippingRequestId",
                table: "ShippingRequestDetails");

            migrationBuilder.AddColumn<int>(
                name: "ShippingPlanId",
                table: "ShippingRequestDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestDetails_ShippingPlanId",
                table: "ShippingRequestDetails",
                column: "ShippingPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRequestDetails_ShippingRequests_ShippingPlanId",
                table: "ShippingRequestDetails",
                column: "ShippingPlanId",
                principalTable: "ShippingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
