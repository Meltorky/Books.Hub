using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editbookentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookCover",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "BookCoverURL",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookFileURL",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookCoverURL",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookFileURL",
                table: "Books");

            migrationBuilder.AddColumn<byte[]>(
                name: "BookCover",
                table: "Books",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
