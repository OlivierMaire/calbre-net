using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class PasskeyProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUsedDate",
                table: "UserCredentials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUsedDevice",
                table: "UserCredentials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUsedIpAddress",
                table: "UserCredentials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUsedLocation",
                table: "UserCredentials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderName",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUsedDate",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "LastUsedDevice",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "LastUsedIpAddress",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "LastUsedLocation",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "ProviderName",
                table: "UserCredentials");
        }
    }
}
