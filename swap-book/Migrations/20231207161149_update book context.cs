using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class updatebookcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Books_BooksBookId",
                table: "BookWishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Wishlists_WishlistsId",
                table: "BookWishlist");

            migrationBuilder.RenameColumn(
                name: "WishlistsId",
                table: "BookWishlist",
                newName: "WishlistId");

            migrationBuilder.RenameColumn(
                name: "BooksBookId",
                table: "BookWishlist",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookWishlist_WishlistsId",
                table: "BookWishlist",
                newName: "IX_BookWishlist_WishlistId");

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

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookWishlist1_WishlistsId",
                table: "BookWishlist1",
                column: "WishlistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Books_BookId",
                table: "BookWishlist",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Wishlists_WishlistId",
                table: "BookWishlist",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Books_BookId",
                table: "BookWishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_BookWishlist_Wishlists_WishlistId",
                table: "BookWishlist");

            migrationBuilder.DropTable(
                name: "BookWishlist1");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.RenameColumn(
                name: "WishlistId",
                table: "BookWishlist",
                newName: "WishlistsId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookWishlist",
                newName: "BooksBookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookWishlist_WishlistId",
                table: "BookWishlist",
                newName: "IX_BookWishlist_WishlistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Books_BooksBookId",
                table: "BookWishlist",
                column: "BooksBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookWishlist_Wishlists_WishlistsId",
                table: "BookWishlist",
                column: "WishlistsId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
