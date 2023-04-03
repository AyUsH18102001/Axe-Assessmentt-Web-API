using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class Question
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        public string QuestionContent { get; set; }

        [Required]
        [MaxLength(150)]
        public string QuestionsType { get; set;}

        public List<Option> Options { get; set;} = new List<Option>();

    }
}
