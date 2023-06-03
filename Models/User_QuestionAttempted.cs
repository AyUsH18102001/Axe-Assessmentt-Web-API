using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class User_QuestionAttempted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionAttemptedId { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public int QuestionId { get; set; }
        public int? OptionIndex { get; set; }

        public string? SqlQuery { get; set; } = null;
        public bool? SqlResult { get; set; } = null;

    }
}
