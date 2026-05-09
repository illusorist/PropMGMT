using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PropertyManagement.Infrastructure.Data;

#nullable disable

namespace PropertyManagement.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260507120000_AddCommercialListingsAndResidentialSeekers")]
public partial class AddCommercialListingsAndResidentialSeekers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "commercial_listings",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                row_flag = table.Column<string>(type: "text", nullable: true),
                serial_number = table.Column<string>(type: "text", nullable: true),
                contact_date = table.Column<string>(type: "text", nullable: true),
                property_status = table.Column<string>(type: "text", nullable: true),
                brokerage_contract = table.Column<string>(type: "text", nullable: true),
                license_number = table.Column<string>(type: "text", nullable: true),
                contract_expiry = table.Column<string>(type: "text", nullable: true),
                ad_number = table.Column<string>(type: "text", nullable: true),
                employee = table.Column<string>(type: "text", nullable: true),
                broker = table.Column<string>(type: "text", nullable: true),
                owner_name = table.Column<string>(type: "text", nullable: true),
                mobile1 = table.Column<string>(type: "text", nullable: true),
                mobile2 = table.Column<string>(type: "text", nullable: true),
                available_units = table.Column<string>(type: "text", nullable: true),
                deed_number = table.Column<string>(type: "text", nullable: true),
                property_type = table.Column<string>(type: "text", nullable: true),
                rooms_count = table.Column<string>(type: "text", nullable: true),
                building_age = table.Column<string>(type: "text", nullable: true),
                has_elevator = table.Column<string>(type: "text", nullable: true),
                other_details = table.Column<string>(type: "text", nullable: true),
                rent_amount = table.Column<string>(type: "text", nullable: true),
                payment_type = table.Column<string>(type: "text", nullable: true),
                location = table.Column<string>(type: "text", nullable: true),
                coordinates = table.Column<string>(type: "text", nullable: true),
                has_key = table.Column<string>(type: "text", nullable: true),
                published_tahmid = table.Column<string>(type: "text", nullable: true),
                published_board = table.Column<string>(type: "text", nullable: true),
                published_designs = table.Column<string>(type: "text", nullable: true),
                published_haraj = table.Column<string>(type: "text", nullable: true),
                published_deal = table.Column<string>(type: "text", nullable: true),
                published_aqar = table.Column<string>(type: "text", nullable: true),
                published_bayut = table.Column<string>(type: "text", nullable: true),
                published_dhaki = table.Column<string>(type: "text", nullable: true),
                published_whatsapp = table.Column<string>(type: "text", nullable: true),
                published_twitter = table.Column<string>(type: "text", nullable: true),
                published_whatsapp_group = table.Column<string>(type: "text", nullable: true),
                published_whatsapp_channel = table.Column<string>(type: "text", nullable: true),
                published_snapchat = table.Column<string>(type: "text", nullable: true),
                published_x = table.Column<string>(type: "text", nullable: true),
                published_instagram = table.Column<string>(type: "text", nullable: true),
                published_tiktok = table.Column<string>(type: "text", nullable: true),
                notes = table.Column<string>(type: "text", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_commercial_listings", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "residential_seekers",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                serial_number = table.Column<string>(type: "text", nullable: true),
                request_date = table.Column<string>(type: "text", nullable: true),
                status = table.Column<string>(type: "text", nullable: true),
                employee = table.Column<string>(type: "text", nullable: true),
                receiver = table.Column<string>(type: "text", nullable: true),
                source_channel = table.Column<string>(type: "text", nullable: true),
                mobile = table.Column<string>(type: "text", nullable: true),
                full_name = table.Column<string>(type: "text", nullable: true),
                nationality = table.Column<string>(type: "text", nullable: true),
                profession = table.Column<string>(type: "text", nullable: true),
                family_count = table.Column<string>(type: "text", nullable: true),
                request_description = table.Column<string>(type: "text", nullable: true),
                max_budget = table.Column<string>(type: "text", nullable: true),
                payment_type = table.Column<string>(type: "text", nullable: true),
                preferred_location = table.Column<string>(type: "text", nullable: true),
                notes = table.Column<string>(type: "text", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_residential_seekers", x => x.id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "commercial_listings");
        migrationBuilder.DropTable(name: "residential_seekers");
    }
}
