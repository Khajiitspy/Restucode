using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class variant_reference_to_parent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductVariants_tblProducts_ProductEntityId",
                table: "tblProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_tblProductVariants_ProductEntityId",
                table: "tblProductVariants");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "tblProductVariants");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "tblProductVariants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_tblProductVariants_ProductId",
                table: "tblProductVariants",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductVariants_tblProducts_ProductId",
                table: "tblProductVariants",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductVariants_tblProducts_ProductId",
                table: "tblProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_tblProductVariants_ProductId",
                table: "tblProductVariants");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "tblProductVariants");

            migrationBuilder.AddColumn<long>(
                name: "ProductEntityId",
                table: "tblProductVariants",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProductVariants_ProductEntityId",
                table: "tblProductVariants",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductVariants_tblProducts_ProductEntityId",
                table: "tblProductVariants",
                column: "ProductEntityId",
                principalTable: "tblProducts",
                principalColumn: "Id");
        }
    }
}
