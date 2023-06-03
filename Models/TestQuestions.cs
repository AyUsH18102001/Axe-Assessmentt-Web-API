using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class TestQuestions
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestQuestionId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(TestId))]
        public int TestId { get; set; } 

        [JsonIgnore]
        public Test? Test { get; set; } = null;
    }
}
