using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class Addbookcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "BookCategories",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategories", x => new { x.BookId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_BookCategories_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Adventure" },
                    { 2, "Biography" },
                    { 3, "Crime" },
                    { 4, "Drama" },
                    { 5, "Fantasy" },
                    { 6, "Historical Fiction" },
                    { 7, "Horror" },
                    { 8, "Humor" },
                    { 9, "Mystery" },
                    { 10, "Poetry" },
                    { 11, "Romance" },
                    { 12, "Science Fiction" },
                    { 13, "Thriller" },
                    { 14, "Cookbook" },
                    { 15, "Religious" },
                    { 16, "Travel" },
                    { 17, "Adventure" },
                    { 18, "Self-Help" },
                    { 19, "Business" },
                    { 20, "Health" },
                    { 21, "Fitness" },
                    { 22, "Education" },
                    { 23, "History" },
                    { 24, "Biography" },
                    { 25, "Art" },
                    { 26, "Music" },
                    { 27, "Philosophy" },
                    { 28, "Psychology" },
                    { 29, "Technology" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_CategoryId",
                table: "BookCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
