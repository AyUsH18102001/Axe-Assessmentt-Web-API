using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AxeAssessmentToolWebAPI.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(900)]
        public string QuestionContent { get; set; }

        public int? IsSQL { get; set; } = null;

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(700)]
        public string? QuestionImage { get; set; } = null;

        [ForeignKey(nameof(QuestionTypeId))]
        public int? QuestionTypeId { get; set; }

        [JsonIgnore]
        public QuestionType? QuestionType { get; set; } = null;

        [ForeignKey(nameof(TestTypeId))]
        public int? TestTypeId { get; set; }

        [JsonIgnore]
        public TestType? TestType { get; set; } = null;

        public List<Option> Options { get; set;} = new List<Option>();

        public DateTime? D_Date { get; set; }
        public int? D_Id { get; set; }
        public DateTime? U_Date { get; set; }
        public int? U_Id { get; set; }
        public DateTime? I_Date { get; set; }
        public int? I_Id { get; set; }
    }
}
