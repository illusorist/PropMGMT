using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceSeededAdminWithBootstrap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty.
            // This migration updates EF model history to remove HasData seeding for admin users.
            // Existing user rows are preserved; runtime bootstrap handles admin creation when needed.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty.
            // We do not reintroduce static seeded admin credentials on downgrade.
        }
    }
}
