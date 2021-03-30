using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartRevisionClean",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartRevisionRaw",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessRevision",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartRevisionClean",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PartRevisionRaw",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProcessRevision",
                table: "Products");
        }
    }
}
