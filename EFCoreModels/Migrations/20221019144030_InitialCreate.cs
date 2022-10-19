using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreModels.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Name_ru = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Name_ru = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", nullable: true),
                    Base64ImageContent = table.Column<string>(type: "TEXT", nullable: true),
                    Link = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Name_ru = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<byte>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalName = table.Column<string>(type: "TEXT", nullable: true),
                    Name_ru = table.Column<string>(type: "TEXT", nullable: true),
                    ReleaseDate = table.Column<string>(type: "TEXT", nullable: true),
                    Quality = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<string>(type: "TEXT", nullable: true),
                    Duration = table.Column<string>(type: "TEXT", nullable: true),
                    FromTheSeries = table.Column<string>(type: "TEXT", nullable: true),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    ImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<byte>(type: "INTEGER", nullable: true),
                    CountryId = table.Column<byte>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GenresToMovies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MovieId = table.Column<Guid>(type: "TEXT", nullable: true),
                    GenreId = table.Column<byte>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenresToMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenresToMovies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GenresToMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_CategoryId",
                table: "Genres",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GenresToMovies_GenreId",
                table: "GenresToMovies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GenresToMovies_MovieId",
                table: "GenresToMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CategoryId",
                table: "Movies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CountryId",
                table: "Movies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ImageId",
                table: "Movies",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenresToMovies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
