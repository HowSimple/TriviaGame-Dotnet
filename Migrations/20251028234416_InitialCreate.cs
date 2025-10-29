using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TriviaApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adult = table.Column<bool>(type: "boolean", nullable: false),
                    backdrop_path = table.Column<string>(type: "text", nullable: false),
                    genre_ids = table.Column<int[]>(type: "integer[]", nullable: false),
                    original_language = table.Column<string>(type: "text", nullable: false),
                    original_title = table.Column<string>(type: "text", nullable: false),
                    overview = table.Column<string>(type: "text", nullable: false),
                    popularity = table.Column<float>(type: "real", nullable: false),
                    poster_path = table.Column<string>(type: "text", nullable: false),
                    release_date = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "triviaQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionDescription = table.Column<string>(type: "text", nullable: false),
                    QuestionTopic = table.Column<string>(type: "text", nullable: false),
                    WrongAnswers = table.Column<string[]>(type: "text[]", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_triviaQuestions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "triviaQuestions");
        }
    }
}
