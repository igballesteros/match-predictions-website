using System;
using webapi_pred.Models;

namespace webapi_pred.Data
{
    public static class MatchSeeder
    {
        public static Match[] GetMatchSeeds()
        {
            return new Match[]
            {
                // Example matches - adjust dates/times as needed
                new Match
                {
                    MatchId = 1,
                    Team1Id = 11, // Van
                    Team2Id = 1, // Atl
                    MatchDate = new DateTime(2025, 5, 2, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 2,
                    Team1Id = 3, // Car
                    Team2Id = 10, // Tor
                    MatchDate = new DateTime(2025, 5, 2, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 3,
                    Team1Id = 12, // Veg
                    Team2Id = 4, // NY
                    MatchDate = new DateTime(2025, 5, 2, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                // Add more matches following the same pattern
                // You can generate these programmatically if you have a regular schedule
                new Match
                {
                    MatchId = 4,
                    Team1Id = 8, // Min
                    Team2Id = 12, // Veg
                    MatchDate = new DateTime(2025, 5, 3, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 5,
                    Team1Id = 3, // Car
                    Team2Id = 2, // Bos
                    MatchDate = new DateTime(2025, 5, 3, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 6,
                    Team1Id = 11, // Van
                    Team2Id = 9, // Tex
                    MatchDate = new DateTime(2025, 5, 3, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                // Continue with more matches up to 30...
                // You might want to create these in a loop if they follow a pattern
                new Match
                {
                    MatchId = 7,
                    Team1Id = 6, // LAT
                    Team2Id = 7, // Mia
                    MatchDate = new DateTime(2025, 5, 3, 23, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 8,
                    Team1Id = 5, // LAG
                    Team2Id = 7, // Mia
                    MatchDate = new DateTime(2025, 5, 4, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 9,
                    Team1Id = 4, // NY
                    Team2Id = 10, // Tor
                    MatchDate = new DateTime(2025, 5, 4, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 10,
                    Team1Id = 1, // Atl
                    Team2Id = 6, // LAT
                    MatchDate = new DateTime(2025, 5, 4, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 11,
                    Team1Id = 12, // Veg
                    Team2Id = 7, // Mia
                    MatchDate = new DateTime(2025, 5, 9, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 12,
                    Team1Id = 2, // Bos
                    Team2Id = 9, // Tex
                    MatchDate = new DateTime(2025, 5, 9, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 13,
                    Team1Id = 1, // Atl
                    Team2Id = 4, // NY
                    MatchDate = new DateTime(2025, 5, 9, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 14,
                    Team1Id = 7, // Mia
                    Team2Id = 4, // NY
                    MatchDate = new DateTime(2025, 5, 10, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 15,
                    Team1Id = 8, // Min
                    Team2Id = 5, // LAG
                    MatchDate = new DateTime(2025, 5, 10, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 16,
                    Team1Id = 6, // LAT
                    Team2Id = 12, // Veg
                    MatchDate = new DateTime(2025, 5, 10, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 17,
                    Team1Id = 3, // Car
                    Team2Id = 9, // Tex
                    MatchDate = new DateTime(2025, 5, 10, 23, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 18,
                    Team1Id = 2, // Bos
                    Team2Id = 11, // Van
                    MatchDate = new DateTime(2025, 5, 11, 19, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 19,
                    Team1Id = 5, // LAG
                    Team2Id = 1, // Atl
                    MatchDate = new DateTime(2025, 5, 11, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 20,
                    Team1Id = 10, // Tor
                    Team2Id = 8, // Min
                    MatchDate = new DateTime(2025, 5, 11, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 21,
                    Team1Id = 9, // Tex
                    Team2Id = 5, // LAG
                    MatchDate = new DateTime(2025, 5, 16, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 22,
                    Team1Id = 3, // Car
                    Team2Id = 8, // Min
                    MatchDate = new DateTime(2025, 5, 16, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 23,
                    Team1Id = 2, // Bos
                    Team2Id = 6, // LAT
                    MatchDate = new DateTime(2025, 5, 16, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 24,
                    Team1Id = 2, // Veg
                    Team2Id = 1, // Atl
                    MatchDate = new DateTime(2025, 5, 17, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 25,
                    Team1Id = 4, // C9NY
                    Team2Id = 6, // LAT
                    MatchDate = new DateTime(2025, 5, 17, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 26,
                    Team1Id = 2, // Mia
                    Team2Id = 6, // Car
                    MatchDate = new DateTime(2025, 5, 17, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 27,
                    Team1Id = 10, // Tor
                    Team2Id = 11, // Van
                    MatchDate = new DateTime(2025, 5, 17, 23, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 28,
                    Team1Id = 5, // Lag
                    Team2Id = 2, // Bos
                    MatchDate = new DateTime(2025, 5, 18, 19, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 29,
                    Team1Id = 10, // Tor
                    Team2Id = 9, // Tex
                    MatchDate = new DateTime(2025, 5, 18, 20, 30, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                },
                new Match
                {
                    MatchId = 30,
                    Team1Id = 8, // Min
                    Team2Id = 11, // Van
                    MatchDate = new DateTime(2025, 5, 18, 22, 0, 0, DateTimeKind.Utc),
                    Team1Score = 0,
                    Team2Score = 0
                }
            };
        }
    }
}
