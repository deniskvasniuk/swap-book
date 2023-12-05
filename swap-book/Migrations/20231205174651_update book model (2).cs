using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class updatebookmodel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Exchangeable",
                table: "Books",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exchangeable",
                table: "Books");
        }
    }
}
