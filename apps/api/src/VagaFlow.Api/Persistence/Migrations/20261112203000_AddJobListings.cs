using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VagaFlow.Api.Persistence.Migrations;

[DbContext(typeof(VagaFlowDbContext))]
[Migration("20261112203000_AddJobListings")]
public partial class AddJobListings : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "job_listings",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                company = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                original_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                canonical_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                source_portal = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                area = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                work_arrangement = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                published_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                first_seen_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                verification_state = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_job_listings", x => x.id);
            });

        migrationBuilder.CreateIndex(
            name: "ux_job_listings_canonical_url",
            table: "job_listings",
            column: "canonical_url",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "job_listings");
    }
}
