using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addidsforfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookCoverId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookFileId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorImageId",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookCoverId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookFileId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AuthorImageId",
                table: "Authors");
        }
    }
}
