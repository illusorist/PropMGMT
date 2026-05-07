using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PropertyManagement.Infrastructure.Data;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260430095000_AddTenantPropertyLink")]
public partial class AddTenantPropertyLink : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "PropertyId",
            table: "Tenants",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tenants_PropertyId",
            table: "Tenants",
            column: "PropertyId");

        migrationBuilder.AddForeignKey(
            name: "FK_Tenants_Properties_PropertyId",
            table: "Tenants",
            column: "PropertyId",
            principalTable: "Properties",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Tenants_Properties_PropertyId",
            table: "Tenants");

        migrationBuilder.DropIndex(
            name: "IX_Tenants_PropertyId",
            table: "Tenants");

        migrationBuilder.DropColumn(
            name: "PropertyId",
            table: "Tenants");
    }
}