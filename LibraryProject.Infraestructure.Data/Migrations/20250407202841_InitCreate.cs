using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryProject.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LIB");

            migrationBuilder.CreateTable(
                name: "BOOKS",
                schema: "LIB",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AuthorLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BookTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Place = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKS", x => x.BookId);
                });


            migrationBuilder.CreateTable(
                name: "COMMENTS",
                schema: "LIB",
                columns: table => new
                {
                    CommentaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentaryText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CommentaryCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentaryUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    UsuId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMMENTS", x => x.CommentaryId);
                    table.ForeignKey(
                        name: "FK_COMMENTS_BOOKS_BookId",
                        column: x => x.BookId,
                        principalSchema: "LIB",
                        principalTable: "BOOKS",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMMENTS_BookId",
                schema: "LIB",
                table: "COMMENTS",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMMENTS",
                schema: "LIB");

            migrationBuilder.DropTable(
                name: "BOOKS",
                schema: "LIB");
        }
    }
}
