using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class fixdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Wishlists_WishlistId",
                table: "BookWishlist");

            migrationBuilder.DropTable(
                name: "BookWishlist1");

            migrationBuilder.DropIndex(
                name: "IX_BookWishlist_WishlistId",
                table: "BookWishlist");

            migrationBuilder.RenameColumn(
                name: "WishlistId",
                table: "BookWishlist",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_BookId",
                table: "Wishlists",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_BookId",
                table: "Wishlists");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BookWishlist",
                newName: "WishlistId");

            migrationBuilder.CreateTable(
                name: "BookWishlist1",
                columns: table => new
                {
                    BooksBookId = table.Column<int>(type: "int", nullable: false),
                    WishlistsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookWishlist1", x => new { x.BooksBookId, x.WishlistsId });
                    table.ForeignKey(
                        name: "FK_BookWishlist1_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookWishlist1_Wishlists_WishlistsId",
                        column: x => x.WishlistsId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookWishlist_WishlistId",
                table: "BookWishlist",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_BookWishlist1_WishlistsId",
                table: "BookWishlist1",
                column: "WishlistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Wishlists_WishlistId",
                table: "BookWishlist",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
