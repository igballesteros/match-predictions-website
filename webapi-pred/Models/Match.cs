using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webapi_pred.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        // columns
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int? WinnerTeamId { get; set; }
        public DateTime MatchDate { get; set; }

        // foreign keys
        [ForeignKey("Team1Id")]
        //[JsonIgnore]
        public Team? Team1 { get; set; }

        [ForeignKey("Team2Id")]
        //[JsonIgnore]
        public Team? Team2 { get; set; }

        [ForeignKey("WinnerTeamId")]
        //[JsonIgnore]
        public Team? WinnerTeam { get; set; }

        // relations
        //[JsonIgnore]
        public ICollection<Prediction>? Predictions { get; set; }
    }
}