using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class MarkRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadStates",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    BookId = table.Column<uint>(type: "INTEGER", nullable: false),
                    MarkedAsRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadStates", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_ReadStates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadStates");
        }
    }
}
