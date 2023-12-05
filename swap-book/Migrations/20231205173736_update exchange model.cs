using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class updateexchangemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "Person",
                table: "Exchanges");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Exchanges",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "ExchangedBookId",
                table: "Exchanges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Exchanges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exchanges",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_BookId",
                table: "Exchanges",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_UserId",
                table: "Exchanges",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_AspNetUsers_UserId",
                table: "Exchanges",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Books_BookId",
                table: "Exchanges",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_AspNetUsers_UserId",
                table: "Exchanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Books_BookId",
                table: "Exchanges");

            migrationBuilder.DropIndex(
                name: "IX_Exchanges_BookId",
                table: "Exchanges");

            migrationBuilder.DropIndex(
                name: "IX_Exchanges_UserId",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "ExchangedBookId",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exchanges");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Exchanges",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Exchanges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Person",
                table: "Exchanges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
