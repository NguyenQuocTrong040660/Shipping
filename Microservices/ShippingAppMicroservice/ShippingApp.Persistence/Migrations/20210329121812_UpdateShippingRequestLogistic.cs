using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShippingRequestLogistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShippingRequestLogistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ProductId",
                table: "ShippingRequestLogistics",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingRequestLogistics_Products_ProductId",
                table: "ShippingRequestLogistics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingRequestLogistics_Products_ProductId",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestLogistics_ProductId",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShippingRequestLogistics");
        }
    }
}
