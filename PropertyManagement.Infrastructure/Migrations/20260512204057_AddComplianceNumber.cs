using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComplianceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "compliance_number",
                table: "Properties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "compliance_number",
                table: "commercial_listings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "compliance_number",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "compliance_number",
                table: "commercial_listings");
        }
    }
}
