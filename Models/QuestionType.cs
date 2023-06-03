using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AxeAssessmentToolWebAPI.Models
{
    public class QuestionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionTypeId { get; set; }

        [MaxLength(30)]
        public string short_name { get; set; }

        [MaxLength(200)]
        public string Type { get; set; }

        [JsonIgnore]
        public IEnumerable<Question> Question { get; set; }

    }
}
