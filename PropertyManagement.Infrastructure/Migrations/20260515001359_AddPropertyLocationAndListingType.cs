using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyLocationAndListingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Properties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "district",
                table: "Properties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "listing_type",
                table: "Properties",
                type: "text",
                nullable: false,
                defaultValue: "Rental");

            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "Properties",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "district",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "listing_type",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "region",
                table: "Properties");
        }
    }
}
