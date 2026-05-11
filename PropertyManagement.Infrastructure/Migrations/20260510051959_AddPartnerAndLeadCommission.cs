using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerAndLeadCommission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "partners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NationalId = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partners", x => x.Id);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "Leads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionAmount",
                table: "Leads",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommissionStatus",
                table: "Leads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommissionNotes",
                table: "Leads",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_partners_UserId",
                table: "partners",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_PartnerId",
                table: "Leads",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_partners_Users_UserId",
                table: "partners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_partners_PartnerId",
                table: "Leads",
                column: "PartnerId",
                principalTable: "partners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partners_Users_UserId",
                table: "partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_partners_PartnerId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_partners_UserId",
                table: "partners");

            migrationBuilder.DropIndex(
                name: "IX_Leads_PartnerId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CommissionAmount",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CommissionStatus",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CommissionNotes",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "partners");
        }
    }
}
