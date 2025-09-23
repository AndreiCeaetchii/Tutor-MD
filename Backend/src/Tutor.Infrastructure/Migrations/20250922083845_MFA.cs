using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MFA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AppUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorRecoveryCodes",
                table: "AppUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "AppUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorRecoveryCodes",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "AppUsers");
        }
    }
}
