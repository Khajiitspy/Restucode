using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ProductVariantOrderFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_tblOrderItems_ProductId",
                table: "tblOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "tblOrderItems");

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItems_ProductVariantId",
                table: "tblOrderItems",
                column: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProductVariants_ProductVariantId",
                table: "tblOrderItems",
                column: "ProductVariantId",
                principalTable: "tblProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProductVariants_ProductVariantId",
                table: "tblOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_tblOrderItems_ProductVariantId",
                table: "tblOrderItems");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "tblOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItems_ProductId",
                table: "tblOrderItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id");
        }
    }
}
