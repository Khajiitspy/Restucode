using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class explicit_images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductId",
                table: "tblProductImages");

            migrationBuilder.DropIndex(
                name: "IX_tblProductImages_ProductId",
                table: "tblProductImages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "tblProductImages");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductImages_ProductVariantId",
                table: "tblProductImages",
                column: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductVariantId",
                table: "tblProductImages",
                column: "ProductVariantId",
                principalTable: "tblProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductVariantId",
                table: "tblProductImages");

            migrationBuilder.DropIndex(
                name: "IX_tblProductImages_ProductVariantId",
                table: "tblProductImages");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "tblProductImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProductImages_ProductId",
                table: "tblProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductId",
                table: "tblProductImages",
                column: "ProductId",
                principalTable: "tblProductVariants",
                principalColumn: "Id");
        }
    }
}
