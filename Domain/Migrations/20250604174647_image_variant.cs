using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class image_variant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductVariantEntityId",
                table: "tblProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProducts_ProductId",
                table: "tblProductImages");

            migrationBuilder.DropIndex(
                name: "IX_tblProductImages_ProductVariantEntityId",
                table: "tblProductImages");

            migrationBuilder.DropColumn(
                name: "ProductVariantEntityId",
                table: "tblProductImages");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "tblProductImages",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantId",
                table: "tblProductImages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductId",
                table: "tblProductImages",
                column: "ProductId",
                principalTable: "tblProductVariants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductId",
                table: "tblProductImages");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "tblProductImages");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "tblProductImages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantEntityId",
                table: "tblProductImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProductImages_ProductVariantEntityId",
                table: "tblProductImages",
                column: "ProductVariantEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductVariantEntityId",
                table: "tblProductImages",
                column: "ProductVariantEntityId",
                principalTable: "tblProductVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProducts_ProductId",
                table: "tblProductImages",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
