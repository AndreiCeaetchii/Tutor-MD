using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionForBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceSnapshot",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Bookings");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceSnapshot",
                table: "Bookings",
                type: "numeric(8,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
