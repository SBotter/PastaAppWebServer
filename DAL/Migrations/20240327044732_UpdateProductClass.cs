using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class UpdateProductClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Ingredients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackages_ProductId",
                table: "ProductPackages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ProductId",
                table: "Ingredients",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Products_ProductId",
                table: "Ingredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPackages_Products_ProductId",
                table: "ProductPackages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Products_ProductId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPackages_Products_ProductId",
                table: "ProductPackages");

            migrationBuilder.DropIndex(
                name: "IX_ProductPackages_ProductId",
                table: "ProductPackages");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_ProductId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Ingredients");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
