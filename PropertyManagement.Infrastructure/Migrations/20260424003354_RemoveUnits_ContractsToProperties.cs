using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnits_ContractsToProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Units_UnitId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Contracts",
                newName: "PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_UnitId",
                table: "Contracts",
                newName: "IX_Contracts_PropertyId");

            migrationBuilder.Sql(
                "UPDATE \"Contracts\" c " +
                "SET \"PropertyId\" = u.\"PropertyId\" " +
                "FROM \"Units\" u " +
                "WHERE c.\"PropertyId\" = u.\"Id\";");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Properties_PropertyId",
                table: "Contracts",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Properties_PropertyId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Contracts",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_PropertyId",
                table: "Contracts",
                newName: "IX_Contracts_UnitId");

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<int>(type: "integer", nullable: false),
                    Area = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BaseRent = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Floor = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UnitNumber = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_PropertyId",
                table: "Units",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Units_UnitId",
                table: "Contracts",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
