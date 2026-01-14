using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimetableDesigner.Backend.Services.Authentication.Database.Migrations
{
    /// <inheritdoc />
    public partial class Accountchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSaltLeft",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "PasswordSaltRight",
                table: "Accounts",
                newName: "PasswordSalt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Accounts",
                newName: "PasswordSaltRight");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSaltLeft",
                table: "Accounts",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
