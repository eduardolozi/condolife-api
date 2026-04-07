using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Condominiums.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_resident_prospect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResidentProspects",
                schema: "condominium",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Apartment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Block = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CondominiumId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentProspects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResidentProspects_Condominiums_CondominiumId",
                        column: x => x.CondominiumId,
                        principalSchema: "condominium",
                        principalTable: "Condominiums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResidentProspects_CondominiumId",
                schema: "condominium",
                table: "ResidentProspects",
                column: "CondominiumId");

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Apartment", "Block" },
                unique: true,
                filter: "\"Block\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Apartment" },
                unique: true,
                filter: "\"Block\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_ResidentProspects_CondominiumId_Cpf",
                schema: "condominium",
                table: "ResidentProspects",
                columns: new[] { "CondominiumId", "Cpf" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResidentProspects",
                schema: "condominium");
        }
    }
}
