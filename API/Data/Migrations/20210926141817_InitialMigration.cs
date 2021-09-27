using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_Products_ProductId",
                table: "ProductDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail");

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "ProductDetail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductsId",
                table: "ProductDetail",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_Products_ProductsId",
                table: "ProductDetail",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_Products_ProductsId",
                table: "ProductDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetail_ProductsId",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "ProductDetail");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_Products_ProductId",
                table: "ProductDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
