using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swap_book.Migrations
{
    /// <inheritdoc />
    public partial class updatebookmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookLink",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookLink",
                table: "Books");
        }
    }
}
