using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Docs.Migrations
{
    /// <inheritdoc />
    public partial class EditShortcut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Application",
                table: "Shortcuts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullShortcut",
                table: "Shortcuts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Application",
                table: "Shortcuts");

            migrationBuilder.DropColumn(
                name: "FullShortcut",
                table: "Shortcuts");
        }
    }
}
