namespace SharedDtos
{
    // For creating new matches (minimum required data)
    public class CreateMatchDto
    {
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public DateTime MatchDate { get; set; }
    }

    // For updating match results (scores and winner)
    public class UpdateMatchDto
    {
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int? WinnerTeamId { get; set; } // Nullable for draws
    }

    // For displaying match information
    public class MatchDto
    {
        public int MatchId { get; set; }

        // Team references (both ID and full DTO)
        public int Team1Id { get; set; }
        public TeamDto Team1 { get; set; } = null!;

        public int Team2Id { get; set; }
        public TeamDto Team2 { get; set; } = null!;

        // Match details
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public TeamDto? WinnerTeam { get; set; }
        public DateTime MatchDate { get; set; }
        public bool IsCompleted { get; set; }

        // Helper properties for UI
        public bool CanPredict => !IsCompleted && MatchDate > DateTime.UtcNow;
        public string Status =>
            IsCompleted ? "Completed" :
            MatchDate > DateTime.UtcNow ? "Upcoming" : "Live";
    }
}