using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class CustomPages_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderPosition",
                table: "Pages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderPosition",
                table: "Pages");
        }
    }
}
