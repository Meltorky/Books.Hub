using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editauthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorImage",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "AuthorImageURL",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorImageURL",
                table: "Authors");

            migrationBuilder.AddColumn<byte[]>(
                name: "AuthorImage",
                table: "Authors",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
