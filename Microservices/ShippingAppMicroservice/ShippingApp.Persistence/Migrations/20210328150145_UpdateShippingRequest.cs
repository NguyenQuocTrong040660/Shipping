using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShippingRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ShippingRequestLogisticId",
                table: "ShippingRequests");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "ShippingRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductLine",
                table: "ShippingRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BillTo",
                table: "ShippingRequestLogistics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillToAddress",
                table: "ShippingRequestLogistics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension",
                table: "ShippingRequestLogistics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Forwarder",
                table: "ShippingRequestLogistics",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "NetWeight",
                table: "ShippingRequestLogistics",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ShipTo",
                table: "ShippingRequestLogistics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress",
                table: "ShippingRequestLogistics",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics",
                column: "ShippingRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ProductLine",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "BillTo",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "BillToAddress",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "Dimension",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "Forwarder",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "NetWeight",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ShipTo",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ShipToAddress",
                table: "ShippingRequestLogistics");

            migrationBuilder.AddColumn<int>(
                name: "ShippingRequestLogisticId",
                table: "ShippingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics",
                column: "ShippingRequestId",
                unique: true);
        }
    }
}
