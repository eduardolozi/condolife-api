using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class delete_duplicated_user_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "identity",
                table: "Invites",
                newName: "CondominiumMembership");

            migrationBuilder.RenameColumn(
                name: "CondominiumId",
                schema: "identity",
                table: "Invites",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "identity",
                table: "CondominiumMemberships",
                newName: "UserRole");

            migrationBuilder.AddColumn<string>(
                name: "TokenHash",
                schema: "identity",
                table: "Invites",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenHash",
                schema: "identity",
                table: "Invites");

            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "identity",
                table: "Invites",
                newName: "CondominiumId");

            migrationBuilder.RenameColumn(
                name: "CondominiumMembership",
                schema: "identity",
                table: "Invites",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                schema: "identity",
                table: "CondominiumMemberships",
                newName: "Role");
        }
    }
}
