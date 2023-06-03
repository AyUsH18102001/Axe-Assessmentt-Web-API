using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class SQL_Answer
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
            
        [Required]
        public string Query { get; set; }


        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(QuestionId))]
        public int QuestionId { get; set; }

        [JsonIgnore]
        public SQL_Question? SQL_Question { get; set; } = null;

    }
}
