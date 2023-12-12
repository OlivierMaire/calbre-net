using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class UserCredentialsUpdateAaguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AaGuid",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AaGuid",
                table: "UserCredentials");
        }
    }
}
