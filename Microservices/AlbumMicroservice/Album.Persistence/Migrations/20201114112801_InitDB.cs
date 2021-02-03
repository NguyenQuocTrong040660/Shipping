using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Album.Persistence.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(maxLength: 50, nullable: false, defaultValueSql: "(newid())"),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoHomePages",
                columns: table => new
                {
                    Id = table.Column<Guid>(maxLength: 50, nullable: false, defaultValueSql: "(newid())"),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    YoutubeLink = table.Column<string>(maxLength: 256, nullable: true),
                    YoutubeId = table.Column<string>(maxLength: 100, nullable: true),
                    YoutubeImage = table.Column<string>(maxLength: 256, nullable: true),
                    Descriptions = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoHomePages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(maxLength: 50, nullable: false, defaultValueSql: "(newid())"),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(maxLength: 256, nullable: true),
                    FileType = table.Column<string>(maxLength: 256, nullable: true),
                    FilePath = table.Column<string>(maxLength: 256, nullable: true),
                    FileUrl = table.Column<string>(maxLength: 500, nullable: true),
                    FileSize = table.Column<long>(nullable: true),
                    AttachmentTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_AttachmentTypes_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AttachmentTypeId",
                table: "Attachments",
                column: "AttachmentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "VideoHomePages");

            migrationBuilder.DropTable(
                name: "AttachmentTypes");
        }
    }
}
