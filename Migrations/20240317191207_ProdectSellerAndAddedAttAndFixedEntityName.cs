using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShoppingApp.Migrations
{
    /// <inheritdoc />
    public partial class ProdectSellerAndAddedAttAndFixedEntityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productSellers_Products_ProductId",
                table: "productSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_productSellers_Users_UserId",
                table: "productSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Buyers_BuyerId",
                table: "Rate");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Products_ProductId",
                table: "Rate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productSellers",
                table: "productSellers");

            migrationBuilder.DropIndex(
                name: "IX_productSellers_ProductId",
                table: "productSellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rate",
                table: "Rate");

            migrationBuilder.RenameTable(
                name: "productSellers",
                newName: "ProductSellers");

            migrationBuilder.RenameTable(
                name: "Rate",
                newName: "Rates");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductSellers",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_BuyerId",
                table: "Rates",
                newName: "IX_Rates_BuyerId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VAT",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSellers",
                table: "ProductSellers",
                columns: new[] { "ProductId", "SellerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                columns: new[] { "ProductId", "BuyerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSellers_SellerId",
                table: "ProductSellers",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellers_Products_ProductId",
                table: "ProductSellers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellers_Sellers_SellerId",
                table: "ProductSellers",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Buyers_BuyerId",
                table: "Rates",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Products_ProductId",
                table: "Rates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellers_Products_ProductId",
                table: "ProductSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellers_Sellers_SellerId",
                table: "ProductSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Buyers_BuyerId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Products_ProductId",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSellers",
                table: "ProductSellers");

            migrationBuilder.DropIndex(
                name: "IX_ProductSellers_SellerId",
                table: "ProductSellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VAT",
                table: "Sellers");

            migrationBuilder.RenameTable(
                name: "ProductSellers",
                newName: "productSellers");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "productSellers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_BuyerId",
                table: "Rate",
                newName: "IX_Rate_BuyerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productSellers",
                table: "productSellers",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rate",
                table: "Rate",
                columns: new[] { "ProductId", "BuyerId" });

            migrationBuilder.CreateIndex(
                name: "IX_productSellers_ProductId",
                table: "productSellers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_productSellers_Products_ProductId",
                table: "productSellers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productSellers_Users_UserId",
                table: "productSellers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Buyers_BuyerId",
                table: "Rate",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Products_ProductId",
                table: "Rate",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
