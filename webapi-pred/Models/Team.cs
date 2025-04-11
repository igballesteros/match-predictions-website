using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webapi_pred.Models
{
    public class Team
    {
        // primary key
        [Key]
        public int TeamId { get; set; }

        // columns
        [Required]
        public string? Teamname { get; set; }

        //[JsonIgnore]
        public ICollection<Match>? MatchesAsTeam1 { get; set; }

        //[JsonIgnore]
        public ICollection<Match>? MatchesAsTeam2 { get; set; }

        //[JsonIgnore]
        public ICollection<Match>? MatchesAsWinner { get; set; }
    }
}