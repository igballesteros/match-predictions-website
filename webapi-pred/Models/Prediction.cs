using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        //[JsonIgnore]
        public User User { get; set; } = null!;

        [ForeignKey("MatchId")]
        //[JsonIgnore]
        public Match Match { get; set; } = null!;

        [ForeignKey("PredictedWinnerId")]
        //[JsonIgnore]
        public Team? PredictedWinner { get; set; }
    }
}