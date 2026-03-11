using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Index_TblUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Users_ExternalId",
                schema: "identity",
                table: "Users",
                newName: "Index_Users_ExternalId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                schema: "identity",
                table: "Users",
                newName: "Index_Users_Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "Index_Users_ExternalId",
                schema: "identity",
                table: "Users",
                newName: "IX_Users_ExternalId");

            migrationBuilder.RenameIndex(
                name: "Index_Users_Email",
                schema: "identity",
                table: "Users",
                newName: "IX_Users_Email");
        }
    }
}
