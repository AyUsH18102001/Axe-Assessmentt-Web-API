using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class Option
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required] 
        public string Answer { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(QuestionId))]
        public int QuestionId { get; set; }

        [JsonIgnore]
        public Question? Question { get; set; }
    }
}
