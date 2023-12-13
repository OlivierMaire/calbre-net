using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class PasskeyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserCredentials",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CredentialId",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CredentialId",
                table: "UserCredentials");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
