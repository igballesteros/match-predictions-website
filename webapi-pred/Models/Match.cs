using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace webapi_pred.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        public int Team1Id { get; set; }
        public int Team2Id { get; set; }

        [ForeignKey("Team1Id")]
        public Team? Team1 { get; set; }

        [ForeignKey("Team2Id")]
        public Team? Team2 { get; set; }

        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;

        public int? WinnerTeamId { get; set; }

        [ForeignKey("WinnerTeamId")]
        public Team? WinnerTeam { get; set; }

        public DateTime MatchDate { get; set; }
        public bool IsCompleted => WinnerTeamId.HasValue;

        public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}