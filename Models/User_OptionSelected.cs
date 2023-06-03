using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AxeAssessmentToolWebAPI.FrontendModel;

namespace AxeAssessmentToolWebAPI.Models
{
    public class User_OptionSelected
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionSelctedId { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(QuestionId))]
        public int QuestionId { get; set; }

        [JsonIgnore]
        public User_QuestionAttempted? QuestionAttempted { get; set; }

        public int OptionIndex { get; set; }
    }
}
