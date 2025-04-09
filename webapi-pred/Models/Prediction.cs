using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi_pred.Models
{
    public class Prediction
    {
        // primary key
        [Key]
        public int PredictionId { get; set; }

        // columns
        [Required]
        public int UserId { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        public int PredictedWinnerId { get; set; }

        public int PredictedTeam1Score { get; set; }
        public int PredictedTeam2Score { get; set; }

        // foreign keys
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("MatchId")]
        public Match? Match { get; set; }

        [ForeignKey("PredictedWinnerId")]
        public Team? PredictedWinner { get; set; }
    }
}