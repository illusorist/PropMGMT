using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PropertyManagement.Infrastructure.Data;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260430090000_AddEmployeeScreenPermissions")]
public partial class AddEmployeeScreenPermissions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ScreenPermissionsJson",
            table: "Users",
            type: "text",
            nullable: false,
            defaultValue: "[]");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ScreenPermissionsJson",
            table: "Users");
    }
}