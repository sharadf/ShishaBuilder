using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShishaBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToTobaccos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tobaccos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tobaccos");
        }
    }
}
