using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PropertyManagement.Infrastructure.Data;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260503130000_AddRequests")]
public partial class AddRequests : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Requests",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Status = table.Column<string>(type: "text", nullable: false),
                Employee = table.Column<string>(type: "text", nullable: false),
                Via = table.Column<string>(type: "text", nullable: false),
                FullName = table.Column<string>(type: "text", nullable: false),
                Nationality = table.Column<string>(type: "text", nullable: false),
                Profession = table.Column<string>(type: "text", nullable: false),
                BedroomCount = table.Column<int>(type: "integer", nullable: true),
                MobileNumber = table.Column<string>(type: "text", nullable: false),
                RequestType = table.Column<string>(type: "text", nullable: false),
                MaxBudget = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                PaymentType = table.Column<string>(type: "text", nullable: false),
                Location = table.Column<string>(type: "text", nullable: false),
                Notes = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Requests", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Requests");
    }
}