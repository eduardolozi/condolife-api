using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Name_LastName_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "identity",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "identity",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "identity",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "identity",
                table: "Users",
                newName: "FullName");
        }
    }
}
