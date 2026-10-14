using EQ.WebApi.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQ.WebApi.Infrastructure.EfCore.Migrations;

[DbContext(typeof(DatabaseContext))]
[Migration("20261014145400_ServiceAssignedToSingleChapter")]
public partial class ServiceAssignedToSingleChapter : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM "chapter_services"
                    GROUP BY "ServiceId"
                    HAVING COUNT(*) > 1
                ) THEN
                    RAISE EXCEPTION 'A service is assigned to multiple chapters. Resolve duplicate chapter_services rows before applying this migration.';
                END IF;
            END $$;
            """);

        migrationBuilder.DropIndex(
            name: "IX_chapter_services_ServiceId",
            table: "chapter_services");

        migrationBuilder.CreateIndex(
            name: "IX_chapter_services_ServiceId",
            table: "chapter_services",
            column: "ServiceId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_chapter_services_ServiceId",
            table: "chapter_services");

        migrationBuilder.CreateIndex(
            name: "IX_chapter_services_ServiceId",
            table: "chapter_services",
            column: "ServiceId");
    }
}
