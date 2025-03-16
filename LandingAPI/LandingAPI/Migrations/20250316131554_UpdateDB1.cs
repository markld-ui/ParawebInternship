using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandingAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Events_EventId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_News_NewsId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_EventId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_NewsId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "EventFiles",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFiles", x => new { x.EventId, x.FileId });
                    table.ForeignKey(
                        name: "FK_EventFiles_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsFiles",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsFiles", x => new { x.NewsId, x.FileId });
                    table.ForeignKey(
                        name: "FK_NewsFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsFiles_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFiles_FileId",
                table: "EventFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsFiles_FileId",
                table: "NewsFiles",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFiles");

            migrationBuilder.DropTable(
                name: "NewsFiles");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Files_EventId",
                table: "Files",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_NewsId",
                table: "Files",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Events_EventId",
                table: "Files",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_News_NewsId",
                table: "Files",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "NewsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
