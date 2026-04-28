using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeadAndPropertyPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RentPrice",
                table: "Properties",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Properties",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ListedPrice",
                table: "Leads",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentPrice",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ListedPrice",
                table: "Leads");
        }
    }
}
