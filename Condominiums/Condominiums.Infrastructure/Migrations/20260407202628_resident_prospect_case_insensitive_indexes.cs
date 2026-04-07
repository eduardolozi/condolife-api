using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Condominiums.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class resident_prospect_case_insensitive_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.DropIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock",
                schema: "condominium",
                table: "ResidentProspects");

            migrationBuilder.DropIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock",
                schema: "condominium",
                table: "ResidentProspects");

            migrationBuilder.AlterColumn<string>(
                name: "Block",
                schema: "condominium",
                table: "ResidentProspects",
                type: "citext",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apartment",
                schema: "condominium",
                table: "ResidentProspects",
                type: "citext",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Apartment" },
                unique: true,
                filter: "\"Block\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Apartment", "Block" },
                unique: true,
                filter: "\"Block\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock",
                schema: "condominium",
                table: "ResidentProspects");

            migrationBuilder.DropIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock",
                schema: "condominium",
                table: "ResidentProspects");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Block",
                schema: "condominium",
                table: "ResidentProspects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apartment",
                schema: "condominium",
                table: "ResidentProspects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Apartment" },
                unique: true,
                filter: "\"Block\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Apartment", "Block" },
                unique: true,
                filter: "\"Block\" IS NOT NULL");
        }
    }
}
