using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShishaBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTobaccos_Tobaccos_TobaccoId",
                table: "OrderTobaccos");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTobaccos_Tobaccos_TobaccoId",
                table: "OrderTobaccos",
                column: "TobaccoId",
                principalTable: "Tobaccos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTobaccos_Tobaccos_TobaccoId",
                table: "OrderTobaccos");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTobaccos_Tobaccos_TobaccoId",
                table: "OrderTobaccos",
                column: "TobaccoId",
                principalTable: "Tobaccos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
