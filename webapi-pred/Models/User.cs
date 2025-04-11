using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webapi_pred.Models
{
    public class User
    {
        // primary key
        [Key]
        public int UserId { get; set; }
        // columns
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public int? Points { get; set; }

        // relations
        //[JsonIgnore]
        public ICollection<Prediction>? Predictions { get; set; }
    }
}