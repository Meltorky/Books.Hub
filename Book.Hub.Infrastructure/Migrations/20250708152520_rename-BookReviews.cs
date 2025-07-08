using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameBookReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReview_Books_BookId",
                table: "BookReview");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReview_Users_UserId",
                table: "BookReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookReview",
                table: "BookReview");

            migrationBuilder.RenameTable(
                name: "BookReview",
                newName: "BookReviews");

            migrationBuilder.RenameIndex(
                name: "IX_BookReview_UserId_BookId",
                table: "BookReviews",
                newName: "IX_BookReviews_UserId_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookReview_BookId",
                table: "BookReviews",
                newName: "IX_BookReviews_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookReviews",
                table: "BookReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReviews_Books_BookId",
                table: "BookReviews",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReviews_Users_UserId",
                table: "BookReviews",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReviews_Books_BookId",
                table: "BookReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReviews_Users_UserId",
                table: "BookReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookReviews",
                table: "BookReviews");

            migrationBuilder.RenameTable(
                name: "BookReviews",
                newName: "BookReview");

            migrationBuilder.RenameIndex(
                name: "IX_BookReviews_UserId_BookId",
                table: "BookReview",
                newName: "IX_BookReview_UserId_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookReviews_BookId",
                table: "BookReview",
                newName: "IX_BookReview_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookReview",
                table: "BookReview",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReview_Books_BookId",
                table: "BookReview",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReview_Users_UserId",
                table: "BookReview",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
