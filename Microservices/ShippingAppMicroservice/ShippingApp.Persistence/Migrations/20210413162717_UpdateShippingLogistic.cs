using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShippingLogistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPackages",
                table: "ShippingRequestLogistics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPackages",
                table: "ShippingRequestLogistics");
        }
    }
}
