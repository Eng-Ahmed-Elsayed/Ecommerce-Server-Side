using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItem_Favorites_FavoriteId",
                table: "FavoriteItem");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItem_Products_ProductId",
                table: "FavoriteItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteItem",
                table: "FavoriteItem");

            migrationBuilder.RenameTable(
                name: "FavoriteItem",
                newName: "FavoriteItems");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteItem_ProductId",
                table: "FavoriteItems",
                newName: "IX_FavoriteItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteItem_FavoriteId",
                table: "FavoriteItems",
                newName: "IX_FavoriteItems_FavoriteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteItems",
                table: "FavoriteItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItems_Favorites_FavoriteId",
                table: "FavoriteItems",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItems_Products_ProductId",
                table: "FavoriteItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItems_Favorites_FavoriteId",
                table: "FavoriteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItems_Products_ProductId",
                table: "FavoriteItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteItems",
                table: "FavoriteItems");

            migrationBuilder.RenameTable(
                name: "FavoriteItems",
                newName: "FavoriteItem");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteItems_ProductId",
                table: "FavoriteItem",
                newName: "IX_FavoriteItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteItems_FavoriteId",
                table: "FavoriteItem",
                newName: "IX_FavoriteItem_FavoriteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteItem",
                table: "FavoriteItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItem_Favorites_FavoriteId",
                table: "FavoriteItem",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItem_Products_ProductId",
                table: "FavoriteItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
