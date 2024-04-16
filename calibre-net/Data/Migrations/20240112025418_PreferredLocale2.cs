using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class PreferredLocale2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredLocale",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredLocale",
                table: "Users");
        }
    }
}
