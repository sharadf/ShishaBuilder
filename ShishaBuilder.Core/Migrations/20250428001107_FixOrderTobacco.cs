using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShishaBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderTobacco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderTobaccos_TobaccoId",
                table: "OrderTobaccos",
                column: "TobaccoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTobaccos_Tobaccos_TobaccoId",
                table: "OrderTobaccos",
                column: "TobaccoId",
                principalTable: "Tobaccos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTobaccos_Tobaccos_TobaccoId",
                table: "OrderTobaccos");

            migrationBuilder.DropIndex(
                name: "IX_OrderTobaccos_TobaccoId",
                table: "OrderTobaccos");
        }
    }
}
