using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBuyerLeadsToGenericLeads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BuyerLeads",
                newName: "Leads");

            migrationBuilder.RenameIndex(
                name: "IX_BuyerLeads_PropertyId",
                table: "Leads",
                newName: "IX_Leads_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyerLeads_AssignedToUserId",
                table: "Leads",
                newName: "IX_Leads_AssignedToUserId");

            migrationBuilder.AddColumn<string>(
                name: "Intent",
                table: "Leads",
                type: "text",
                nullable: false,
                defaultValue: "Buy");

            migrationBuilder.Sql("UPDATE \"Leads\" SET \"Intent\" = 'Buy' WHERE \"Intent\" = '';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Intent",
                table: "Leads");

            migrationBuilder.RenameTable(
                name: "Leads",
                newName: "BuyerLeads");

            migrationBuilder.RenameIndex(
                name: "IX_Leads_PropertyId",
                table: "BuyerLeads",
                newName: "IX_BuyerLeads_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Leads_AssignedToUserId",
                table: "BuyerLeads",
                newName: "IX_BuyerLeads_AssignedToUserId");
        }
    }
}
