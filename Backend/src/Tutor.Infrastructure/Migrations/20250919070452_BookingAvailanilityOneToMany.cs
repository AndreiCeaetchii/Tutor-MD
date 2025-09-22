using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookingAvailanilityOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AvailabilityRuleId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AvailabilityRuleId",
                table: "Bookings",
                column: "AvailabilityRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AvailabilityRuleId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AvailabilityRuleId",
                table: "Bookings",
                column: "AvailabilityRuleId",
                unique: true);
        }
    }
}
