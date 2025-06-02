using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class name_tbl_ingredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductIngredients_Ingredients_IngredientId",
                table: "tblProductIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "tblIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblIngredients",
                table: "tblIngredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductIngredients_tblIngredients_IngredientId",
                table: "tblProductIngredients",
                column: "IngredientId",
                principalTable: "tblIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductIngredients_tblIngredients_IngredientId",
                table: "tblProductIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblIngredients",
                table: "tblIngredients");

            migrationBuilder.RenameTable(
                name: "tblIngredients",
                newName: "Ingredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductIngredients_Ingredients_IngredientId",
                table: "tblProductIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
