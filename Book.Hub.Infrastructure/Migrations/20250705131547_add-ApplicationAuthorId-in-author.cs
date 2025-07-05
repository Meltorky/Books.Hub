using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addApplicationAuthorIdinauthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationAuthorId",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationAuthorId",
                table: "Authors");
        }
    }
}
