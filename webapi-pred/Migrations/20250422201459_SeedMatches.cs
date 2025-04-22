using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi_pred.Migrations
{
    /// <inheritdoc />
    public partial class SeedMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "MatchId", "MatchDate", "Team1Id", "Team1Score", "Team2Id", "Team2Score", "WinnerTeamId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 2, 19, 0, 0, 0, DateTimeKind.Utc), 11, 0, 1, 0, null },
                    { 2, new DateTime(2025, 5, 2, 20, 30, 0, 0, DateTimeKind.Utc), 3, 0, 10, 0, null },
                    { 3, new DateTime(2025, 5, 2, 22, 0, 0, 0, DateTimeKind.Utc), 12, 0, 4, 0, null },
                    { 4, new DateTime(2025, 5, 3, 19, 0, 0, 0, DateTimeKind.Utc), 8, 0, 12, 0, null },
                    { 5, new DateTime(2025, 5, 3, 20, 30, 0, 0, DateTimeKind.Utc), 3, 0, 2, 0, null },
                    { 6, new DateTime(2025, 5, 3, 22, 0, 0, 0, DateTimeKind.Utc), 11, 0, 9, 0, null },
                    { 7, new DateTime(2025, 5, 3, 23, 30, 0, 0, DateTimeKind.Utc), 6, 0, 7, 0, null },
                    { 8, new DateTime(2025, 5, 4, 19, 0, 0, 0, DateTimeKind.Utc), 5, 0, 7, 0, null },
                    { 9, new DateTime(2025, 5, 4, 20, 30, 0, 0, DateTimeKind.Utc), 4, 0, 10, 0, null },
                    { 10, new DateTime(2025, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), 1, 0, 6, 0, null },
                    { 11, new DateTime(2025, 5, 9, 19, 0, 0, 0, DateTimeKind.Utc), 12, 0, 7, 0, null },
                    { 12, new DateTime(2025, 5, 9, 20, 30, 0, 0, DateTimeKind.Utc), 2, 0, 9, 0, null },
                    { 13, new DateTime(2025, 5, 9, 22, 0, 0, 0, DateTimeKind.Utc), 1, 0, 4, 0, null },
                    { 14, new DateTime(2025, 5, 10, 19, 0, 0, 0, DateTimeKind.Utc), 7, 0, 4, 0, null },
                    { 15, new DateTime(2025, 5, 10, 20, 30, 0, 0, DateTimeKind.Utc), 8, 0, 5, 0, null },
                    { 16, new DateTime(2025, 5, 10, 22, 0, 0, 0, DateTimeKind.Utc), 6, 0, 12, 0, null },
                    { 17, new DateTime(2025, 5, 10, 23, 30, 0, 0, DateTimeKind.Utc), 3, 0, 9, 0, null },
                    { 18, new DateTime(2025, 5, 11, 19, 30, 0, 0, DateTimeKind.Utc), 2, 0, 11, 0, null },
                    { 19, new DateTime(2025, 5, 11, 20, 30, 0, 0, DateTimeKind.Utc), 5, 0, 1, 0, null },
                    { 20, new DateTime(2025, 5, 11, 22, 0, 0, 0, DateTimeKind.Utc), 10, 0, 8, 0, null },
                    { 21, new DateTime(2025, 5, 16, 19, 0, 0, 0, DateTimeKind.Utc), 9, 0, 5, 0, null },
                    { 22, new DateTime(2025, 5, 16, 20, 30, 0, 0, DateTimeKind.Utc), 3, 0, 8, 0, null },
                    { 23, new DateTime(2025, 5, 16, 22, 0, 0, 0, DateTimeKind.Utc), 2, 0, 6, 0, null },
                    { 24, new DateTime(2025, 5, 17, 19, 0, 0, 0, DateTimeKind.Utc), 2, 0, 1, 0, null },
                    { 25, new DateTime(2025, 5, 17, 20, 30, 0, 0, DateTimeKind.Utc), 4, 0, 6, 0, null },
                    { 26, new DateTime(2025, 5, 17, 22, 0, 0, 0, DateTimeKind.Utc), 2, 0, 6, 0, null },
                    { 27, new DateTime(2025, 5, 17, 23, 30, 0, 0, DateTimeKind.Utc), 10, 0, 11, 0, null },
                    { 28, new DateTime(2025, 5, 18, 19, 0, 0, 0, DateTimeKind.Utc), 5, 0, 2, 0, null },
                    { 29, new DateTime(2025, 5, 18, 20, 30, 0, 0, DateTimeKind.Utc), 10, 0, 9, 0, null },
                    { 30, new DateTime(2025, 5, 18, 22, 0, 0, 0, DateTimeKind.Utc), 8, 0, 11, 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "MatchId",
                keyValue: 30);
        }
    }
}
