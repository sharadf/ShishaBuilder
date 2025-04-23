using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShishaBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Masters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Masters");
        }
    }
}
