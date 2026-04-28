using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    public partial class RemoveAgencyOwnerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Permanently remove users that had the AgencyOwner role.
            migrationBuilder.Sql("DELETE FROM \"Users\" WHERE \"Role\" = 'AgencyOwner';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down migration is intentionally left empty — deleted accounts cannot be restored.
        }
    }
}
