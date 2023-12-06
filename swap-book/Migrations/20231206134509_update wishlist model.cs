using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class updatewishlistmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_BookId",
                table: "Wishlists");

            migrationBuilder.CreateTable(
                name: "BookWishlist",
                columns: table => new
                {
                    BooksBookId = table.Column<int>(type: "int", nullable: false),
                    WishlistsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookWishlist", x => new { x.BooksBookId, x.WishlistsId });
                    table.ForeignKey(
                        name: "FK_BookWishlist_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookWishlist_Wishlists_WishlistsId",
                        column: x => x.WishlistsId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookWishlist_WishlistsId",
                table: "BookWishlist",
                column: "WishlistsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookWishlist");

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
    }
}
