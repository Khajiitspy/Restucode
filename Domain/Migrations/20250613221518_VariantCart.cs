using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class VariantCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_tblProductVariants_ProductId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Carts",
                newName: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_tblProductVariants_ProductVariantId",
                table: "Carts",
                column: "ProductVariantId",
                principalTable: "tblProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_tblProductVariants_ProductVariantId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "Carts",
                newName: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_tblProductVariants_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "tblProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
