using System.ComponentModel.DataAnnotations;

namespace webapi_pred.DTOs
{
    public class PredictionDto
    {
        public int PredictionId { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public MatchDto Match { get; set; } = null!;
        public int PredictedWinnerId { get; set; }
        public TeamDto PredictedWinner { get; set; } = null!;
        public int PredictedTeam1Score { get; set; }
        public int PredictedTeam2Score { get; set; }
    }

    public class CreatePredictionDto
    {
        [Required]
        public int MatchId { get; set; }

        [Required]
        public int PredictedWinnerId { get; set; }

        [Required]
        public int PredictedTeam1Score { get; set; }

        [Required]
        public int PredictedTeam2Score { get; set; }
    }

    public class UpdatePredictionDto
    {
        [Required]
        public int PredictedWinnerId { get; set; }

        [Required]
        public int PredictedTeam1Score { get; set; }

        public int PredictedTeam2Score { get; set; }
    }
}