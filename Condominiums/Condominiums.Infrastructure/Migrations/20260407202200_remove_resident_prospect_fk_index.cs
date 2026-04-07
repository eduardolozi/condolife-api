using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Condominiums.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_resident_prospect_fk_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP INDEX IF EXISTS condominium."IX_ResidentProspects_CondominiumId";
                """);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX IF NOT EXISTS "UX_ResidentProspects_CondominiumId_Cpf"
                ON condominium."ResidentProspects" ("CondominiumId", "Cpf");
                """);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX IF NOT EXISTS "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock"
                ON condominium."ResidentProspects" ("CondominiumId", "Apartment")
                WHERE "Block" IS NULL;
                """);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX IF NOT EXISTS "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock"
                ON condominium."ResidentProspects" ("CondominiumId", "Apartment", "Block")
                WHERE "Block" IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ResidentProspects_CondominiumId",
                schema: "condominium",
                table: "ResidentProspects",
                column: "CondominiumId");
        }
    }
}
