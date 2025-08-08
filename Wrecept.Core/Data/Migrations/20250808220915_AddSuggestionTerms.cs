using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wrecept.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSuggestionTerms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuggestionTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Term = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Frequency = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUsedUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestionTerms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionTerms_LastUsedUtc",
                table: "SuggestionTerms",
                column: "LastUsedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionTerms_Term",
                table: "SuggestionTerms",
                column: "Term",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuggestionTerms");
        }
    }
}
