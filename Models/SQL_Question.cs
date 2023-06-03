using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AxeAssessmentToolWebAPI.Models
{
    public class SQL_Question
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        public string QuestionContent { get; set; }

        [Required]
        public string QuestionTitle { get; set; }

        public SQL_Answer SQL_Answer { get; set; }
    }
}
