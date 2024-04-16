using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibre_net.Migrations
{
    /// <inheritdoc />
    public partial class UserCredentialsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AaGuid",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "AttestationClientDataJson",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "AttestationFormat",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "AttestationObject",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "Descriptor",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "DevicePublicKeys",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "IsBackedUp",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "IsBackupEligible",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "RegDate",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "SignCount",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "UserHandle",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "UserIdBytes",
                table: "UserCredentials");

            migrationBuilder.RenameColumn(
                name: "Transports",
                table: "UserCredentials",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CredentialId",
                table: "UserCredentials",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "UserCredentials",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "UserCredentials");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "UserCredentials",
                newName: "Transports");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CredentialId",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AaGuid",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "AttestationClientDataJson",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "AttestationFormat",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "AttestationObject",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Descriptor",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DevicePublicKeys",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBackedUp",
                table: "UserCredentials",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBackupEligible",
                table: "UserCredentials",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "PublicKey",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RegDate",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<uint>(
                name: "SignCount",
                table: "UserCredentials",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<byte[]>(
                name: "UserHandle",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "UserIdBytes",
                table: "UserCredentials",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
