using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class product_variiants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductIngredients_tblProducts_ProductId",
                table: "tblProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryId",
                table: "tblProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblProductSizes_ProductSizeId",
                table: "tblProducts");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_CategoryId",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "tblProducts");

            migrationBuilder.RenameColumn(
                name: "ProductSizeId",
                table: "tblProducts",
                newName: "ProductSizeEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_tblProducts_ProductSizeId",
                table: "tblProducts",
                newName: "IX_tblProducts_ProductSizeEntityId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "tblProductIngredients",
                newName: "ProductVariantId");

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantEntityId",
                table: "tblProductImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblProductVariants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ProductSizeId = table.Column<long>(type: "bigint", nullable: true),
                    ProductEntityId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProductVariants_tblCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tblCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblProductVariants_tblProductSizes_ProductSizeId",
                        column: x => x.ProductSizeId,
                        principalTable: "tblProductSizes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblProductVariants_tblProducts_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "tblProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblProductImages_ProductVariantEntityId",
                table: "tblProductImages",
                column: "ProductVariantEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductVariants_CategoryId",
                table: "tblProductVariants",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductVariants_ProductEntityId",
                table: "tblProductVariants",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductVariants_ProductSizeId",
                table: "tblProductVariants",
                column: "ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductVariantEntityId",
                table: "tblProductImages",
                column: "ProductVariantEntityId",
                principalTable: "tblProductVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductIngredients_tblProductVariants_ProductVariantId",
                table: "tblProductIngredients",
                column: "ProductVariantId",
                principalTable: "tblProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblProductSizes_ProductSizeEntityId",
                table: "tblProducts",
                column: "ProductSizeEntityId",
                principalTable: "tblProductSizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProductVariants_ProductVariantEntityId",
                table: "tblProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProductIngredients_tblProductVariants_ProductVariantId",
                table: "tblProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblProductSizes_ProductSizeEntityId",
                table: "tblProducts");

            migrationBuilder.DropTable(
                name: "tblProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_tblProductImages_ProductVariantEntityId",
                table: "tblProductImages");

            migrationBuilder.DropColumn(
                name: "ProductVariantEntityId",
                table: "tblProductImages");

            migrationBuilder.RenameColumn(
                name: "ProductSizeEntityId",
                table: "tblProducts",
                newName: "ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_tblProducts_ProductSizeEntityId",
                table: "tblProducts",
                newName: "IX_tblProducts_ProductSizeId");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "tblProductIngredients",
                newName: "ProductId");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "tblProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "tblProducts",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "tblProducts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "tblProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_CategoryId",
                table: "tblProducts",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductIngredients_tblProducts_ProductId",
                table: "tblProductIngredients",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryId",
                table: "tblProducts",
                column: "CategoryId",
                principalTable: "tblCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblProductSizes_ProductSizeId",
                table: "tblProducts",
                column: "ProductSizeId",
                principalTable: "tblProductSizes",
                principalColumn: "Id");
        }
    }
}
